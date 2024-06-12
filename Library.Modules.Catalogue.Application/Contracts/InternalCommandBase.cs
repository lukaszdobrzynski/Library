namespace Library.Modules.Catalogue.Application.Contracts;

public abstract class InternalCommandBase
{
    public string Id { get; set; }
    protected abstract string Name  { get; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime LastFailedAt { get; set; }
    public string ProcessingError { get; set; }
    public DateTime ProcessedAt { get; set; }
    public InternalCommandStatus Status { get; set; }

    private readonly List<string> _validationErrors = new();

    public void AddValidationError(string fieldName, string message)
    {
        var fullMessage = $"[{fieldName}]: {message}";
        _validationErrors.Add(fullMessage);  
    } 
    
    public string GetSignature()
        => $"Internal Command ID: {Id}. Command name: {Name}.";
    
    public virtual void Validate()
    {
        if (_validationErrors.Count == 0) 
            return;
        
        var errors = string.Join(Environment.NewLine, _validationErrors);
        var signature = GetSignature();
        throw new InvalidInternalCommandException($"INTERNAL COMMAND validation failed ({signature}){Environment.NewLine}{errors}");
    }
}