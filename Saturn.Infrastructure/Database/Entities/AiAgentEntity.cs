namespace Saturn.Infrastructure.Database.Entities;

public class AiAgentEntity
{
    public Guid Id { get; set; }

    public required string Name { get; set; }

    public required string Code { get; set; }
    
    public required string Prompt { get; set; }

    public virtual List<ChatEntity>? Chats { get; set; }
    
}