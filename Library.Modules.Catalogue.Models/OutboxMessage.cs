namespace Library.Modules.Catalogue.Models;

public class OutboxMessage
{
    public Guid Id { get; set; }
    public DateTime OccurredOn { get; set; }
    public DateTime? ProcessedAt { get; set; }
    public string Type { get; set; }
    public string Data { get; set; }

    public OutboxMessage(Guid id, DateTime occurredOn, string type, string data)
    {
        Id = id;
        OccurredOn = occurredOn;
        Type = type;
        Data = data;
    }
}