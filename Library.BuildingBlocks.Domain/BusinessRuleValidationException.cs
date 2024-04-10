namespace Library.BuildingBlocks.Domain;

public class BusinessRuleValidationException : Exception
{
    public IBusinessRule BrokenRule { get; set; }
    public string Details { get; set; }

    public BusinessRuleValidationException(IBusinessRule brokenRule)
    {
        BrokenRule = brokenRule;
        Details = brokenRule.Message;
    }

    public override string ToString() => $"{BrokenRule.GetType().FullName}: {Details}";
}