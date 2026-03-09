namespace Shared.Exceptions;

/// <summary>
/// Exception thrown when validation fails
/// </summary>
public class ValidationException : DomainException
{
    public Dictionary<string, string[]> Errors { get; }

    public ValidationException(Dictionary<string, string[]> errors)
        : base("One or more validation errors occurred", "VALIDATION_ERROR")
    {
        Errors = errors;
        AddMetadata("errors", errors);
    }

    public ValidationException(string propertyName, string errorMessage)
        : base($"Validation failed for '{propertyName}': {errorMessage}", "VALIDATION_ERROR")
    {
        Errors = new Dictionary<string, string[]>
        {
            { propertyName, new[] { errorMessage } }
        };
        AddMetadata("errors", Errors);
    }
}
