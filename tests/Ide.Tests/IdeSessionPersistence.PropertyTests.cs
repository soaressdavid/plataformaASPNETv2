using FsCheck;
using FsCheck.Xunit;
using Shared.Models;
using System.Text.Json;

namespace Ide.Tests;

/// <summary>
/// Property-based tests for IDE session persistence functionality.
/// Feature: platform-evolution-saas
/// </summary>
public class IdeSessionPersistencePropertyTests
{
    /// <summary>
    /// Property 8: IDE Session Persistence Round Trip
    /// **Validates: Requirements 3.14**
    /// 
    /// For any IDE session state (open files, cursor positions, editor content), 
    /// saving the session and then restoring it SHALL produce an equivalent session state.
    /// 
    /// This property ensures that:
    /// 1. All file paths are preserved
    /// 2. All file contents are preserved
    /// 3. All file languages are preserved
    /// 4. Active file selection is preserved
    /// 5. All cursor positions are preserved
    /// </summary>
    [Property(MaxTest = 100)]
    public Property SessionPersistence_ShouldRoundTrip()
    {
        return Prop.ForAll(
            IdeSessionStateGenerator(),
            originalSession =>
            {
                // Act - Serialize and deserialize (simulating save/load)
                var serialized = JsonSerializer.Serialize(originalSession);
                var restored = JsonSerializer.Deserialize<IdeSessionState>(serialized);

                // Assert - Verify all properties are preserved
                var isValid = restored != null &&
                    VerifyOpenFilesMatch(originalSession.OpenFiles, restored.OpenFiles) &&
                    VerifyActiveFileMatches(originalSession.ActiveFile, restored.ActiveFile) &&
                    VerifyCursorPositionsMatch(originalSession.CursorPositions, restored.CursorPositions);

                return isValid
                    .Label($"Session should round-trip correctly. " +
                           $"Original files: {originalSession.OpenFiles.Count}, " +
                           $"Restored files: {restored?.OpenFiles.Count ?? 0}, " +
                           $"Active file: {originalSession.ActiveFile ?? "null"}");
            });
    }

    /// <summary>
    /// Property: Empty session should round-trip correctly
    /// 
    /// An empty IDE session (no open files, no active file, no cursor positions)
    /// should serialize and deserialize correctly.
    /// </summary>
    [Property(MaxTest = 20)]
    public void EmptySession_ShouldRoundTrip()
    {
        // Arrange
        var emptySession = new IdeSessionState
        {
            OpenFiles = new List<IdeFile>(),
            ActiveFile = null,
            CursorPositions = new Dictionary<string, CursorPosition>()
        };

        // Act
        var serialized = JsonSerializer.Serialize(emptySession);
        var restored = JsonSerializer.Deserialize<IdeSessionState>(serialized);

        // Assert
        Assert.NotNull(restored);
        Assert.Empty(restored.OpenFiles);
        Assert.Null(restored.ActiveFile);
        Assert.Empty(restored.CursorPositions);
    }

    /// <summary>
    /// Property: Session with single file should round-trip correctly
    /// </summary>
    [Property(MaxTest = 50)]
    public Property SingleFileSession_ShouldRoundTrip(NonEmptyString filePath, NonEmptyString content, NonEmptyString language)
    {
        return Prop.ForAll(
            Gen.Choose(0, 1000).ToArbitrary(),
            Gen.Choose(0, 200).ToArbitrary(),
            (line, column) =>
            {
                // Arrange
                var file = new IdeFile
                {
                    Path = filePath.Get,
                    Content = content.Get,
                    Language = language.Get
                };

                var session = new IdeSessionState
                {
                    OpenFiles = new List<IdeFile> { file },
                    ActiveFile = filePath.Get,
                    CursorPositions = new Dictionary<string, CursorPosition>
                    {
                        [filePath.Get] = new CursorPosition { Line = line, Column = column }
                    }
                };

                // Act
                var serialized = JsonSerializer.Serialize(session);
                var restored = JsonSerializer.Deserialize<IdeSessionState>(serialized);

                // Assert
                var isValid = restored != null &&
                    restored.OpenFiles.Count == 1 &&
                    restored.OpenFiles[0].Path == file.Path &&
                    restored.OpenFiles[0].Content == file.Content &&
                    restored.OpenFiles[0].Language == file.Language &&
                    restored.ActiveFile == filePath.Get &&
                    restored.CursorPositions.ContainsKey(filePath.Get) &&
                    restored.CursorPositions[filePath.Get].Line == line &&
                    restored.CursorPositions[filePath.Get].Column == column;

                return isValid.Label("Single file session should preserve all properties");
            });
    }

    /// <summary>
    /// Property: Session with special characters in content should round-trip correctly
    /// 
    /// File content may contain special characters, unicode, newlines, etc.
    /// All content should be preserved exactly.
    /// </summary>
    [Property(MaxTest = 50)]
    public void SessionWithSpecialCharacters_ShouldRoundTrip()
    {
        // Arrange - Create session with various special characters
        var specialContent = "Hello\nWorld\r\n\tTabbed\n\"Quoted\"\n'Single'\n\\Backslash\n{Braces}\n[Brackets]\nUnicode: 你好 🚀";
        
        var session = new IdeSessionState
        {
            OpenFiles = new List<IdeFile>
            {
                new IdeFile
                {
                    Path = "test.cs",
                    Content = specialContent,
                    Language = "csharp"
                }
            },
            ActiveFile = "test.cs",
            CursorPositions = new Dictionary<string, CursorPosition>
            {
                ["test.cs"] = new CursorPosition { Line = 5, Column = 10 }
            }
        };

        // Act
        var serialized = JsonSerializer.Serialize(session);
        var restored = JsonSerializer.Deserialize<IdeSessionState>(serialized);

        // Assert
        Assert.NotNull(restored);
        Assert.Single(restored.OpenFiles);
        Assert.Equal(specialContent, restored.OpenFiles[0].Content);
    }

    /// <summary>
    /// Property: Large file content should round-trip correctly
    /// 
    /// IDE sessions may contain large files. The persistence mechanism
    /// should handle large content without data loss.
    /// </summary>
    [Property(MaxTest = 20)]
    public void SessionWithLargeContent_ShouldRoundTrip()
    {
        // Arrange - Create a large file (10KB of content)
        var largeContent = string.Join("\n", Enumerable.Range(0, 500).Select(i => 
            $"public class Class{i} {{ public void Method{i}() {{ Console.WriteLine(\"Line {i}\"); }} }}"));
        
        var session = new IdeSessionState
        {
            OpenFiles = new List<IdeFile>
            {
                new IdeFile
                {
                    Path = "LargeFile.cs",
                    Content = largeContent,
                    Language = "csharp"
                }
            },
            ActiveFile = "LargeFile.cs",
            CursorPositions = new Dictionary<string, CursorPosition>
            {
                ["LargeFile.cs"] = new CursorPosition { Line = 250, Column = 50 }
            }
        };

        // Act
        var serialized = JsonSerializer.Serialize(session);
        var restored = JsonSerializer.Deserialize<IdeSessionState>(serialized);

        // Assert
        Assert.NotNull(restored);
        Assert.Single(restored.OpenFiles);
        Assert.Equal(largeContent, restored.OpenFiles[0].Content);
        Assert.Equal(largeContent.Length, restored.OpenFiles[0].Content.Length);
    }

    /// <summary>
    /// Property: Session with no active file should round-trip correctly
    /// 
    /// It's valid to have open files but no active file selected.
    /// </summary>
    [Property(MaxTest = 30)]
    public Property SessionWithNoActiveFile_ShouldRoundTrip()
    {
        return Prop.ForAll(
            NonEmptyFileListGenerator(),
            files =>
            {
                // Arrange
                var session = new IdeSessionState
                {
                    OpenFiles = files,
                    ActiveFile = null,
                    CursorPositions = new Dictionary<string, CursorPosition>()
                };

                // Act
                var serialized = JsonSerializer.Serialize(session);
                var restored = JsonSerializer.Deserialize<IdeSessionState>(serialized);

                // Assert
                var isValid = restored != null &&
                    restored.OpenFiles.Count == files.Count &&
                    restored.ActiveFile == null;

                return isValid.Label("Session with no active file should preserve all files");
            });
    }

    #region Helper Methods

    /// <summary>
    /// Verifies that two lists of IdeFile objects match
    /// </summary>
    private bool VerifyOpenFilesMatch(List<IdeFile> original, List<IdeFile> restored)
    {
        if (original.Count != restored.Count)
            return false;

        for (int i = 0; i < original.Count; i++)
        {
            if (original[i].Path != restored[i].Path ||
                original[i].Content != restored[i].Content ||
                original[i].Language != restored[i].Language)
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Verifies that active file paths match (including null case)
    /// </summary>
    private bool VerifyActiveFileMatches(string? original, string? restored)
    {
        return original == restored;
    }

    /// <summary>
    /// Verifies that cursor position dictionaries match
    /// </summary>
    private bool VerifyCursorPositionsMatch(
        Dictionary<string, CursorPosition> original,
        Dictionary<string, CursorPosition> restored)
    {
        if (original.Count != restored.Count)
            return false;

        foreach (var kvp in original)
        {
            if (!restored.ContainsKey(kvp.Key))
                return false;

            var originalPos = kvp.Value;
            var restoredPos = restored[kvp.Key];

            if (originalPos.Line != restoredPos.Line ||
                originalPos.Column != restoredPos.Column)
            {
                return false;
            }
        }

        return true;
    }

    #endregion

    #region Generators

    /// <summary>
    /// Generates random IdeSessionState objects for property testing
    /// </summary>
    private static Arbitrary<IdeSessionState> IdeSessionStateGenerator()
    {
        return Arb.From(
            from fileCount in Gen.Choose(0, 10)
            from files in Gen.ListOf(fileCount, IdeFileGenerator().Generator)
            from activeFileIndex in Gen.Choose(-1, fileCount - 1)
            from cursorCount in Gen.Choose(0, fileCount)
            select CreateSessionState(files.ToList(), activeFileIndex, cursorCount)
        );
    }

    /// <summary>
    /// Creates an IdeSessionState with the given parameters
    /// </summary>
    private static IdeSessionState CreateSessionState(List<IdeFile> files, int activeFileIndex, int cursorCount)
    {
        var session = new IdeSessionState
        {
            OpenFiles = files,
            ActiveFile = activeFileIndex >= 0 && activeFileIndex < files.Count 
                ? files[activeFileIndex].Path 
                : null,
            CursorPositions = new Dictionary<string, CursorPosition>()
        };

        // Add cursor positions for some files
        for (int i = 0; i < Math.Min(cursorCount, files.Count); i++)
        {
            session.CursorPositions[files[i].Path] = new CursorPosition
            {
                Line = i * 10,
                Column = i * 5
            };
        }

        return session;
    }

    /// <summary>
    /// Generates random IdeFile objects
    /// </summary>
    private static Arbitrary<IdeFile> IdeFileGenerator()
    {
        var languages = new[] { "csharp", "sql", "json", "xml", "typescript", "javascript" };
        
        return Arb.From(
            from fileName in Arb.Generate<NonEmptyString>()
            from extension in Gen.Elements(".cs", ".sql", ".json", ".xml", ".ts", ".js")
            from content in Arb.Generate<string>()
            from language in Gen.Elements(languages)
            select new IdeFile
            {
                Path = $"{fileName.Get}{extension}",
                Content = content ?? string.Empty,
                Language = language
            }
        );
    }

    /// <summary>
    /// Generates a non-empty list of IdeFile objects
    /// </summary>
    private static Arbitrary<List<IdeFile>> NonEmptyFileListGenerator()
    {
        return Arb.From(
            from fileCount in Gen.Choose(1, 10)
            from files in Gen.ListOf(fileCount, IdeFileGenerator().Generator)
            select files.ToList()
        );
    }

    #endregion
}
