namespace Odering.Infrastructure.Processing.Outbox;

public class OutBoxMessage
{
    public Guid Id { get; set; }
    public DateTime OccurredOn { get; set; }
    public string Type { get; set; }
    public string Data { get; set; }
    public DateTime? ProcessedDate { get; set; }
    private OutBoxMessage()
    {
        // Parameterless constructor for EF Core
    }

    public OutBoxMessage(DateTime occurredOn, string type, string data)
    {
        Id = Guid.NewGuid();
        OccurredOn = occurredOn;
        Type = type;
        Data = data;
    }
}