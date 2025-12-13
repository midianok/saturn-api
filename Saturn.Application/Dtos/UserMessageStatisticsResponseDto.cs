namespace Saturn.Application.Dtos;

public class UserMessageStatisticsResponseDto
{
    public string? UserName { get; init; }

    public required int MessagesCount { get; init; }
    
    public required int VoiceCount { get; init; }

    public required int VideoNoteCount { get; init; }

    public required int PhotoCount { get; init; }
    
    public required int StickerCount { get; init; }
    
    public required int AnimationCount { get; init; }
    
    public required int VideoCount { get; init; }
}