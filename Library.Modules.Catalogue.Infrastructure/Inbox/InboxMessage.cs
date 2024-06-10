namespace Library.Modules.Catalogue.Infrastructure.Inbox;

public class InboxMessage
{
    public string Id { get; set; }
    public DateTime OccurredOn { get; set; }
    public string Type { get; set; }
    public string Data { get; set; }
    public string ProcessingError { get; set; }
    public DateTime? LastFailedAt { get; set; }

    public InboxMessageProcessingStatus Status { get; set; }

    public static InboxMessage CreateSubmitted(DateTime occurredOn, string type, string data)
    {
        return new InboxMessage
        {
            OccurredOn = occurredOn,
            Type = type,
            Data = data,
            Status = InboxMessageProcessingStatus.Submitted
        };
    }
}