using AutoMapper;
using Saturn.Application.Dtos;
using Saturn.Domain.Model;

namespace Saturn.Application.Mapping;

public class TelegramMessageMappingProfile : Profile
{
    public TelegramMessageMappingProfile()
    {
        CreateMap<SaveMessageDto, TelegramMessage>()
            .ForMember(d => d.Id, o => o.MapFrom(s => s.MessageId))
            .ForMember(d => d.Text, o => o.MapFrom(s => s.MessageText))
            .ForMember(d => d.Date, o => o.MapFrom(s => s.MessageDate))
            .ForMember(d => d.Type, o => o.MapFrom(s => s.MessageType))
            .ForMember(d => d.Chat, o => o.MapFrom(s => new TelegramChat
            {
                Id = s.ChatId,
                Name = s.ChatName,
                Type = s.ChatType
            }))
            .ForMember(d => d.From, o =>
            {
                o.PreCondition(s => s.FromId != 0);
                o.MapFrom(s => new TelegramUser
                {
                    Id = s.FromId,
                    FirstName = s.FromFirstName ?? string.Empty,
                    LastName = s.FromLastName ?? string.Empty,
                    Username = s.FromUsername ?? string.Empty
                });
            })
            .ForMember(d => d.Sticker, o =>
            {
                o.PreCondition(s => !string.IsNullOrEmpty(s.StickerFileId));
                o.MapFrom(s => new TelegramSticker
                {
                    FileId = s.StickerFileId!
                });
            })
            .ForMember(d => d.ReplyToMessage, o => o.Ignore());;
    }
}