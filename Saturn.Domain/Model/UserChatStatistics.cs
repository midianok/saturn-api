namespace Saturn.Domain.Model;

public record UserChatStatistics(long UserId, string? Username, string? FirstName, string? LastName, int MessageCount);