namespace Shared.ValueObjects;

/// <summary>
/// Value object representing a step within a guided project.
/// </summary>
public record ProjectStep(
    int Order,
    string Title,
    string Instructions,
    string StarterCode,
    string ValidationCriteria
);
