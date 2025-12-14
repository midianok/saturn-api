using Saturn.Application.Dtos;
using Saturn.Domain.Model;

namespace Saturn.Application.Mappers;

public static class UserMessageStatisticsMap
{
    public static GetUserMessageStatisticsResponseDto ToGetUserMessageStatisticsResponse(this UserMessageStatistics userMessageStatistics) =>
        new()
        {
            UserName = userMessageStatistics.UserName,
            AnimationCount = userMessageStatistics.AnimationCount,
            MessagesCount = userMessageStatistics.MessagesCount,
            PhotoCount = userMessageStatistics.PhotoCount,
            StickerCount = userMessageStatistics.StickerCount,
            VideoCount = userMessageStatistics.VideoCount,
            VideoNoteCount = userMessageStatistics.VideoNoteCount,
            VoiceCount = userMessageStatistics.VoiceCount,
        };
}