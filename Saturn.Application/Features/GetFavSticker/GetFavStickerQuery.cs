using MediatR;
using Saturn.Application.Dtos;

namespace Saturn.Application.Features.GetFavSticker;

public record GetFavStickerQuery(long UserId, long ChatId) : IRequest<GetFavStickerResponseDto>;