using MediatR;
using Saturn.Application.Dtos;

namespace Saturn.Application.Features.GetUserMessageStatistics;

public class GetUserMessageStatisticsQuery : IRequest<UserMessageStatisticsResponseDto>
{
    public GetUserMessageStatisticsQuery(long userId, long chatId)
    {
        UserId = userId;
        ChatId = chatId;
    }

    public long UserId { get; init; }
    
    public long ChatId { get; init; }
}