namespace Shared.ValueObjects;

/// <summary>
/// Value object representing parsed test case data.
/// This is separate from the TestCase entity which is used for database storage.
/// </summary>
public record TestCaseData(
    string Input,
    string ExpectedOutput,
    bool IsHidden
);
