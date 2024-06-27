namespace Library.Modules.Reservation.Infrastructure.InternalCommands;

public class InternalCommand
{
    public Guid Id { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime? ProcessedAt { get; set; }
    public string Type { get; set; }
    public string Data { get; set; }
    public string ProcessingError { get; set; }

    private InternalCommand()
    {
        // EF only
    }
}