namespace Library.Modules.Catalogue.Models;

public enum OutboxMessageProcessingStatus
{
    Submitted,
    Processing,
    Processed,
    Error
}