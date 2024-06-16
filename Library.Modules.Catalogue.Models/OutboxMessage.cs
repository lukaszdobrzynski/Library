namespace Library.Modules.Catalogue.Models;

public class OutboxMessage
{
    public string Id { get; set; }
    public DateTime OccurredOn { get; set; }
    public DateTime? ProcessedAt { get; set; }
    public string Type { get; set; }
    public string Data { get; set; }
    public DateTime? LastFailedAt { get; set; }
    public string ProcessingError { get; set; }
    public OutboxMessageProcessingStatus Status { get; set; }

    public static OutboxMessage CreateSubmitted(string id, DateTime occurredOn, string type, string data)
    {
        return new OutboxMessage
        {
            Id = id,
            OccurredOn = occurredOn,
            Type = type,
            Data = data,
            Status = OutboxMessageProcessingStatus.Submitted
        };
    }
}