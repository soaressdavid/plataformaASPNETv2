namespace Shared.Exceptions;

/// <summary>
/// Exception thrown when a business rule is violated
/// </summary>
public class BusinessRuleException : DomainException
{
    public string RuleName { get; }

    public BusinessRuleException(string ruleName, string message)
        : base(message, "BUSINESS_RULE_VIOLATION")
    {
        RuleName = ruleName;
        AddMetadata("ruleName", ruleName);
    }

    public BusinessRuleException(string ruleName, string message, Exception innerException)
        : base(message, "BUSINESS_RULE_VIOLATION", innerException)
    {
        RuleName = ruleName;
        AddMetadata("ruleName", ruleName);
    }
}
