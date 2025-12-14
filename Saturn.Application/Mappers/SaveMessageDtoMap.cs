using Saturn.Application.Dtos;
using Saturn.Domain.Model;

namespace Saturn.Application.Mappers;

public static class SaveMessageDtoMap
{
    public static TelegramMessage ToTelegramMessage(this SaveMessageDto saveMessageDto)
    {
        var message = new TelegramMessage
        {
            Id = saveMessageDto.MessageId,
            Text = saveMessageDto.MessageText,
            MessageDate = saveMessageDto.MessageDate,
            Type = saveMessageDto.MessageType,
            StickerId = saveMessageDto.StickerId,
            ReplyToMessageId = saveMessageDto.ReplyToMessageId,
            ReplyToMessageChatId = saveMessageDto.ReplyToMessageChatId,
            Chat = new TelegramChat
            {
                Id = saveMessageDto.ChatId,
                Name = saveMessageDto.ChatName,
                Type = saveMessageDto.ChatType
            },
        };
        
        if (saveMessageDto.FromId.HasValue)
        {
            message.From = new TelegramUser
            {
                Id = saveMessageDto.FromId.Value,
                FirstName = saveMessageDto.FromFirstName ?? string.Empty,
                LastName = saveMessageDto.FromLastName ?? string.Empty,
                Username = saveMessageDto.FromUsername ?? string.Empty
            };
        }

        return message;
    }
}