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

    public async Task<UserMessageStatistics> GetUserMessageStatisticsAsync(long userId, long chatId, CancellationToken cancellationToken)
    {
        var db = await _contextFactory.CreateDbContextAsync(cancellationToken);

        var messageTypes = await db.Messages.Where(x => x.ChatId == chatId && x.UserId == userId)
            .Select(x => new { x.Type, x.User!.Username })
            .ToListAsync(cancellationToken: cancellationToken);
        
        return new UserMessageStatistics
        { 
            UserName = messageTypes.FirstOrDefault()?.Username,
            VoiceCount = messageTypes.Count(x => x.Type == (int) MessageType.Voice),
            VideoNoteCount = messageTypes.Count(x => x.Type == (int) MessageType.VideoNote),
            PhotoCount = messageTypes.Count(x => x.Type == (int) MessageType.Photo),
            StickerCount = messageTypes.Count(x => x.Type == (int) MessageType.Sticker),
            AnimationCount = messageTypes.Count(x => x.Type == (int) MessageType.Animation),
            VideoCount = messageTypes.Count(x => x.Type == (int) MessageType.Video)
        };
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
            MessageDate = msg.Date,
            StickerId = msg.Sticker?.FileId,
            UserId = msg.From!.Id,
            ReplyToMessageId = msg.ReplyToMessage?.Id,
            ReplyToMessageChatId = msg.ReplyToMessage?.Chat!.Id,
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