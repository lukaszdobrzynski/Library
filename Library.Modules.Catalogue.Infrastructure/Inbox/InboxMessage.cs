namespace Library.Modules.Catalogue.Infrastructure.Inbox;

public class InboxMessage
{
    public string Id { get; set; }

    public DateTime OccurredOn { get; set; }
    public string Type { get; set; }
    public string Data { get; set; }
}