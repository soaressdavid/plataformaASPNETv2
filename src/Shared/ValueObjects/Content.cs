namespace Shared.ValueObjects;

/// <summary>
/// Value object representing parsed lesson content.
/// </summary>
public record Content(
    string Title,
    string Description,
    List<CodeBlock> CodeBlocks,
    List<string> KeyPoints
);

/// <summary>
/// Represents a code block within lesson content.
/// </summary>
public record CodeBlock(
    string Language,
    string Code,
    string? Caption
);
