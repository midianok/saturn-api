namespace Saturn.Application.Dtos;

public class UserMessageStatisticsResponseDto
{
    public string? UserName { get; init; }
    
    public int VoiceCount { get; init; }

    public int VideoNoteCount { get; init; }

    public int PhotoCount { get; init; }
    
    public int StickerCount { get; init; }
    
    public int AnimationCount { get; init; }
    
    public int VideoCount { get; init; }
}