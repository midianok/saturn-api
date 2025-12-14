namespace Saturn.Application.Dtos;

public record UserChatStatisticsDto
{
    public long UserId { get; init; }
    
    public string? Username { get; init; }
    
    public string? FirstName { get; init; }
    
    public string? LastName { get; init; }

    public int MessageCount { get; init; }
}