using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Saturn.Domain.Model;
using Saturn.Domain.Services;
using Saturn.Infrastructure.Database;
using Saturn.Infrastructure.Database.Entities;

namespace Saturn.Infrastructure.Services;

public class MessageService : IMessageService
{
    private readonly IDbContextFactory<SaturnContext> _contextFactory;
    private readonly SemaphoreSlim _semaphoreSlim = new(1, 1);
    private readonly IMemoryCache _memoryCache;

    public MessageService(IDbContextFactory<SaturnContext> contextFactory, IMemoryCache memoryCache)
    {
        _contextFactory = contextFactory;
        _memoryCache = memoryCache;
    }
    
    public async Task SaveMessageAsync(TelegramMessage telegramMessage, CancellationToken cancellationToken)
    {
        if (telegramMessage.From == null || telegramMessage.Chat == null) return;
        
        await _semaphoreSlim.WaitAsync(cancellationToken);
        try
        {
            await using var db = await _contextFactory.CreateDbContextAsync(cancellationToken);
        
            await ProcessUser(telegramMessage.From, db);
            await ProcessChat(telegramMessage.Chat, db);
            await ProcessMessage(telegramMessage, db);

            await db.SaveChangesAsync(cancellationToken);
        }
        finally
        {
            _semaphoreSlim.Release();
        }
    }

    public async Task<UserMessageStatistics> GetUserMessageStatisticsAsync(long chatId, long userId, DateTime? from, DateTime? to, CancellationToken cancellationToken)
    {
        var db = await _contextFactory.CreateDbContextAsync(cancellationToken);

        var query = db.Messages
            .Where(x => x.ChatId == chatId && x.UserId == userId);

        if (from.HasValue)
            query = query.Where(x => x.MessageDate >= from.Value);

        if (to.HasValue)
            query = query.Where(x => x.MessageDate <= to.Value);

        var messages = await query
            .Select(x => new { x.Type, x.User!.Username })
            .ToListAsync(cancellationToken: cancellationToken);
        
        return new UserMessageStatistics
        { 
            UserName = messages.FirstOrDefault()?.Username,
            MessagesCount = messages.Count,
            VoiceCount = messages.Count(x => x.Type == (int) MessageType.Voice),
            VideoNoteCount = messages.Count(x => x.Type == (int) MessageType.VideoNote),
            PhotoCount = messages.Count(x => x.Type == (int) MessageType.Photo),
            StickerCount = messages.Count(x => x.Type == (int) MessageType.Sticker),
            AnimationCount = messages.Count(x => x.Type == (int) MessageType.Animation),
            VideoCount = messages.Count(x => x.Type == (int) MessageType.Video)
        };
    }

    public async Task<IReadOnlyCollection<UserChatStatistics>> GetChatMessageStatisticsAsync(long chatId, DateTime? from, DateTime? to, CancellationToken cancellationToken)
    {
        var db = await _contextFactory.CreateDbContextAsync(cancellationToken);
        
        var query = db.Messages
            .Where(x => x.ChatId == chatId);

        if (from.HasValue)
            query = query.Where(x => x.MessageDate >= from.Value);

        if (to.HasValue)
            query = query.Where(x => x.MessageDate <= to.Value);

        var messages = await query
            .GroupBy(x => x.UserId)
            .Select(x => new UserChatStatistics(x.Key, x.First().User!.Username, x.First().User!.FirstName, x.First().User!.LastName, x.Count()))
            .OrderByDescending(x => x.MessageCount)
            .ToListAsync(cancellationToken);

        return messages;
    }

    public async Task<string?> GetFavStickerAsync(long userId, long chatId, CancellationToken cancellationToken)
    {
        var db = await _contextFactory.CreateDbContextAsync(cancellationToken);
        var userStickers = await db.Messages
            .Where(x => x.ChatId == chatId && x.UserId == userId &&
                        x.Type == (int) MessageType.Sticker)
            .ToListAsync(cancellationToken: cancellationToken);

        var favSticker = userStickers.GroupBy(x => x.StickerId)
            .OrderByDescending(grp => grp.Count())
            .FirstOrDefault();
        
        return favSticker?.Key;
    }

    private async Task ProcessMessage(TelegramMessage msg, SaturnContext db) => 
        await db.Messages.AddAsync(CreateMessage(msg));
    
    private async Task ProcessChat(TelegramChat tgChat, SaturnContext db)
    {
        var chat = await GetCachedEntityById<ChatEntity>(tgChat.Id, db, TimeSpan.FromHours(30));

        if (chat == null)
        {
            await db.Chats.AddAsync(CreateChat(tgChat));
        }
        else if (chat.Type != tgChat.Type || chat.Name != tgChat.Name)
        {
            db.Chats.Update(CreateChat(tgChat));
            RemoveCachedEntityById<ChatEntity>(tgChat.Id);
        }
    }

    private async Task ProcessUser(TelegramUser tgUser, SaturnContext db)
    {
        var user = await GetCachedEntityById<UserEntity>(tgUser.Id, db, TimeSpan.FromMinutes(30));
        if (user == null)
        {
            await db.Users.AddAsync(CreateUser(tgUser));
        }
        else if (user.FirstName != tgUser.FirstName || user.LastName != tgUser.LastName || user.Username != tgUser.Username)
        {
            db.Users.Update(CreateUser(tgUser));
            RemoveCachedEntityById<UserEntity>(tgUser.Id);
        }
    }

    private async Task<T?> GetCachedEntityById<T>(long id, SaturnContext db, TimeSpan expirationTime) where T : class
    {
        var cacheKey = $"{typeof(T).Name}_{id}";
        if (_memoryCache.TryGetValue(cacheKey, out T? cachedEntity))
        {
            return cachedEntity;
        }
        
        var entity = await db.Set<T>().FindAsync(id);
        if (entity == null)
        {
            return null;
        }
        
        _memoryCache.Set(cacheKey, entity, expirationTime);
        return entity;

    }
    
    private void RemoveCachedEntityById<T>(long id) =>
        _memoryCache.Remove($"{typeof(T).Name}_{id}");

    private static MessageEntity CreateMessage(TelegramMessage msg) =>
        new()
        {
            Id = msg.Id,
            ChatId = msg.Chat!.Id,
            Type = msg.Type,
            Text = msg.Text,
            MessageDate = msg.MessageDate,
            StickerId = msg.StickerId,
            UserId = msg.From!.Id,
            ReplyToMessageId = msg.ReplyToMessageId,
            ReplyToMessageChatId = msg.ReplyToMessageChatId,
        };
    
    private static ChatEntity CreateChat(TelegramChat tgChat) =>
        new()
        {
            Id = tgChat.Id,
            Type = tgChat.Type,
            Name = tgChat.Name
        };

    private static UserEntity CreateUser(TelegramUser tgUser) =>
        new()
        {
            Id = tgUser.Id,
            FirstName = tgUser.FirstName,
            LastName = tgUser.LastName,
            Username = tgUser.Username
        };
}