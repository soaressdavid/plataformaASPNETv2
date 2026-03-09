namespace Shared.Models;

/// <summary>
/// Represents an IDE session state for a user
/// </summary>
public class IdeSession : BaseEntity
{
    public string UserId { get; set; } = string.Empty;
    public string SessionData { get; set; } = string.Empty; // JSON serialized session state
    public DateTime LastSavedAt { get; set; }
}

/// <summary>
/// DTO for IDE session state
/// </summary>
public class IdeSessionState
{
    public List<IdeFile> OpenFiles { get; set; } = new();
    public string? ActiveFile { get; set; }
    public Dictionary<string, CursorPosition> CursorPositions { get; set; } = new();
}

/// <summary>
/// Represents a file in the IDE
/// </summary>
public class IdeFile
{
    public string Path { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string Language { get; set; } = "csharp";
}

/// <summary>
/// Represents cursor position in a file
/// </summary>
public class CursorPosition
{
    public int Line { get; set; }
    public int Column { get; set; }
}
