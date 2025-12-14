using Saturn.Application.Dtos;
using Saturn.Domain.Model;

namespace Saturn.Application.Mappers;

public static class SaveMessageDtoMap
{
    public static TelegramMessage ToTelegramMessage(this SaveMessageDto saveMessageDto) =>
        new()
        {
            Id = saveMessageDto.MessageId,
            Text = saveMessageDto.MessageText,
            MessageDate = saveMessageDto.MessageDate,
            Type = saveMessageDto.MessageType,
            StickerId = saveMessageDto.StickerId,
            ReplyToMessageId = saveMessageDto.ReplyToMessageId,
            ReplyToMessageChatId = saveMessageDto.ReplyToMessageChatId,
            Chat = new TelegramChat(saveMessageDto.ChatId, saveMessageDto.ChatType, saveMessageDto.ChatName),
            From = saveMessageDto.FromId.HasValue ? 
                new TelegramUser(saveMessageDto.FromId.Value, saveMessageDto.FromUsername, saveMessageDto.FromFirstName, saveMessageDto.FromLastName) : null
        };
}