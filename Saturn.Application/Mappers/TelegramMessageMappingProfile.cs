using Mapster;
using Saturn.Application.Dtos;
using Saturn.Domain.Model;

namespace Saturn.Application.Mappers;
 
public class TelegramMessageMappingProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<SaveMessageDto, TelegramMessage>()
            .Map(dest => dest.Id, src => src.MessageId)
            .Map(dest => dest.Text, src => src.MessageText)
            .Map(dest => dest.MessageDate, src => src.MessageDate)
            .Map(dest => dest.Type, src => src.MessageType)
            .Map(dest => dest.StickerId, src => src.StickerId)
            .Map(dest => dest.ReplyToMessageId, src => src.ReplyToMessageId)
            .Map(dest => dest.ReplyToMessageChatId, src => src.ReplyToMessageChatId)
            .Map(dest => dest.Chat, src => new TelegramChat(src.ChatId, src.ChatType, src.ChatName))
            .Map(dest => dest.From, src => src.FromId.HasValue ? new TelegramUser(src.FromId.Value, src.FromUsername, src.FromFirstName, src.FromLastName) : null);
    }
}