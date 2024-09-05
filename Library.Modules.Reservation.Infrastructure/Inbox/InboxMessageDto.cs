namespace Library.Modules.Reservation.Infrastructure.Inbox;

internal class InboxMessageDto
{
    public Guid Id { get; set; }

    public string Type { get; set; }

    public string Data { get; set; }
}