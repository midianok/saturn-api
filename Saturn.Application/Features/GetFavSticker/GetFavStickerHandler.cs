using AutoMapper;
using MediatR;
using Saturn.Application.Dtos;
using Saturn.Domain.Services;

namespace Saturn.Application.Features.GetFavSticker;

public class GetFavStickerHandler : IRequestHandler<GetFavStickerQuery, GetFavStickerResponseDto>
{
    private readonly IMessageService _messageService;
    private readonly IMapper _mapper;

    public GetFavStickerHandler(IMessageService messageService, IMapper mapper)
    {
        _messageService = messageService;
        _mapper = mapper;
    }

    public async Task<GetFavStickerResponseDto> Handle(GetFavStickerQuery request, CancellationToken cancellationToken)
    {
        var favStickerId = await _messageService.GetFavStickerAsync(request.UserId, request.ChatId, cancellationToken);
        return new GetFavStickerResponseDto(favStickerId);
    }
}