namespace Library.Modules.Reservation.Infrastructure.Configuration.Processing;

public class InternalCommandDto
{
    public Guid Id { get; set; }

    public string Type { get; set; }

    public string Data { get; set; }
}