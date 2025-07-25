namespace Odering.Infrastructure.Processing.Outbox;

public class OutboxMessage
{
    public Guid Id { get; set; }
    public DateTime OccurredOn { get; set; }
    public string Type { get; set; }
    public string Data { get; set; }
    public DateTime? ProcessedDate { get; set; }
    private OutboxMessage()
    {
        // Parameterless constructor for EF Core
    }

    public OutboxMessage(DateTime occurredOn, string type, string data)
    {
        Id = Guid.NewGuid();
        OccurredOn = occurredOn.ToUniversalTime();
        Type = type;
        Data = data;
    }
}