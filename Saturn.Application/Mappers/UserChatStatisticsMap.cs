using Saturn.Application.Dtos;
using Saturn.Domain.Model;

namespace Saturn.Application.Mappers;

public static class UserChatStatisticsMap
{
    public static UserChatStatisticsDto ToGetUserMessageStatisticsResponse(this UserChatStatistics userChatStatistics) =>
        new()
        {
            UserId = userChatStatistics.UserId,
            Username = userChatStatistics.Username,
            FirstName = userChatStatistics.FirstName,
            LastName = userChatStatistics.LastName,
            MessageCount = userChatStatistics.MessageCount
        };
}