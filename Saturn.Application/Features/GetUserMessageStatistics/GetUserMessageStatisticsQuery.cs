using MediatR;
using Saturn.Application.Dtos;

namespace Saturn.Application.Features.GetUserMessageStatistics;

public record GetUserMessageStatisticsQuery(long UserId, long ChatId, DateTime? From, DateTime? To) : IRequest<GetUserMessageStatisticsResponseDto>;