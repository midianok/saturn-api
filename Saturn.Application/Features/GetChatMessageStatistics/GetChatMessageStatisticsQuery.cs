using MediatR;
using Saturn.Application.Dtos;

namespace Saturn.Application.Features.GetChatMessageStatistics;

public record GetChatMessageStatisticsQuery(long ChatId, DateTime? From, DateTime? To) : IRequest<GetChatMessageStatisticsResponseDto>;