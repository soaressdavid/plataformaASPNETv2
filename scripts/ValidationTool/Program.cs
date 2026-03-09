using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Shared.Models;
using Shared.Services;
using Shared.Data;
using Shared.Entities;

namespace Scripts;

/// <summary>
/// Phase 2 Checkpoint Validation
/// Validates all 120 lessons across Levels 0-5
/// </summary>
public class ValidatePhase2Checkpoint
{
    public static void Main(string[] args)
    {
        Console.WriteLine("=" + new string('=', 79));
        Console.WriteLine("PHASE 2 CHECKPOINT VALIDATION");
        Console.WriteLine("Levels 0-5 (120 lessons total)");
        Console.WriteLine("=" + new string('=', 79));
        Console.WriteLine();

        var validator = new LessonValidator();
        var allResults = new List<LevelValidationResult>();

        // Validate each level
        allResults.Add(ValidateLevel(0, "Fundamentos de Programação", new Level0ContentSeeder(), validator));
        allResults.Add(ValidateLevel(1, "Programação Orientada a Objetos", new Level1ContentSeeder(), validator));
        allResults.Add(ValidateLevel(2, "Estruturas de Dados e Algoritmos", new Level2ContentSeeder(), validator));
        allResults.Add(ValidateLevel(3, "Banco de Dados e SQL", new Level3ContentSeeder(), validator));
        allResults.Add(ValidateLevel(4, "Entity Framework Core", new Level4ContentSeeder(), validator));
        allResults.Add(ValidateLevel(5, "ASP.NET Core Fundamentos", new Level5ContentSeeder(), validator));

        // Print overall summary
        PrintOverallSummary(allResults);

        // Save results to JSON
        SaveResults(allResults);

        // Determine exit code
        int totalValid = allResults.Sum(r => r.ValidCount);
        if (totalValid == 120)
        {
            Console.WriteLine();
            Console.WriteLine("🎉 SUCCESS! All 120 lessons pass validation!");
            Environment.Exit(0);
        }
        else
        {
            Console.WriteLine();
            Console.WriteLine($"⚠️  {120 - totalValid} lesson(s) need attention");
            Environment.Exit(1);
        }
    }

    private static LevelValidationResult ValidateLevel(
        int levelNumber, 
        string levelTitle, 
        dynamic seeder, 
        LessonValidator validator)
    {
        Console.WriteLine($"📚 LEVEL {levelNumber}: {levelTitle}");
        Console.WriteLine(new string('-', 79));

        var lessons = seeder.GetType()
            .GetMethod($"CreateLevel{levelNumber}Lessons")
            .Invoke(seeder, null) as List<Lesson>;

        var result = new LevelValidationResult
        {
            LevelNumber = levelNumber,
            LevelTitle = levelTitle,
            TotalLessons = lessons.Count,
            LessonResults = new List<LessonValidationResult>()
        };

        foreach (var lesson in lessons.OrderBy(l => l.OrderIndex))
        {
            var content = JsonSerializer.Deserialize<LessonContent>(lesson.StructuredContent);
            var validationResult = validator.Validate(content);

            var lessonResult = new LessonValidationResult
            {
                LessonNumber = lesson.OrderIndex,
                Title = lesson.Title,
                IsValid = validationResult.IsValid,
                Objectives = content.Objectives.Count,
                TheorySections = content.Theory.Count,
                TotalWords = CalculateTotalWords(content),
                CodeExamples = content.CodeExamples.Count,
                Exercises = content.Exercises.Count,
                HasSummary = !string.IsNullOrWhiteSpace(content.Summary),
                Issues = validationResult.Errors
            };

            result.LessonResults.Add(lessonResult);

            if (lessonResult.IsValid)
            {
                result.ValidCount++;
            }
            else
            {
                result.IssueCount++;
                Console.WriteLine($"  ⚠️  Lesson {lessonResult.LessonNumber:D2}: {lessonResult.Title}");
                foreach (var issue in lessonResult.Issues)
                {
                    Console.WriteLine($"       - {issue}");
                }
            }
        }

        // Print level summary
        string statusIcon = result.ValidCount == 20 ? "✅" : "⚠️";
        Console.WriteLine($"{statusIcon} Level {levelNumber} Summary: {result.ValidCount}/20 valid");
        Console.WriteLine();

        return result;
    }

    private static void PrintOverallSummary(List<LevelValidationResult> results)
    {
        Console.WriteLine("=" + new string('=', 79));
        Console.WriteLine("OVERALL STATISTICS");
        Console.WriteLine("=" + new string('=', 79));
        Console.WriteLine();

        // Level-by-level summary
        Console.WriteLine("Level Summary:");
        foreach (var result in results)
        {
            string status = result.ValidCount == 20 ? "✅" : "⚠️";
            Console.WriteLine($"  {status} Level {result.LevelNumber}: {result.ValidCount}/20 valid - {result.LevelTitle}");
        }

        Console.WriteLine();

        // Overall stats
        int totalLessons = results.Sum(r => r.TotalLessons);
        int totalValid = results.Sum(r => r.ValidCount);
        int totalIssues = results.Sum(r => r.IssueCount);
        int totalTheorySections = results.Sum(r => r.LessonResults.Sum(l => l.TheorySections));
        int totalCodeExamples = results.Sum(r => r.LessonResults.Sum(l => l.CodeExamples));
        int totalExercises = results.Sum(r => r.LessonResults.Sum(l => l.Exercises));

        Console.WriteLine("Content Statistics:");
        Console.WriteLine($"  Total lessons: {totalLessons}/120");
        Console.WriteLine($"  ✅ Valid lessons: {totalValid}");
        Console.WriteLine($"  ⚠️  Lessons with issues: {totalIssues}");
        Console.WriteLine($"  Total theory sections: {totalTheorySections}");
        Console.WriteLine($"  Total code examples: {totalCodeExamples}");
        Console.WriteLine($"  Total exercises: {totalExercises}");
        Console.WriteLine();

        double completionPct = (totalValid * 100.0) / 120;
        Console.WriteLine($"📊 Validation Pass Rate: {totalValid}/120 ({completionPct:F1}%)");

        // Requirements validation
        Console.WriteLine();
        Console.WriteLine("=" + new string('=', 79));
        Console.WriteLine("REQUIREMENTS VALIDATION");
        Console.WriteLine("=" + new string('=', 79));

        var reqChecks = new[]
        {
            ("Req 2.1: All required sections present", totalValid == 120 ? "✅ PASS" : "⚠️  PARTIAL"),
            ("Req 2.2: Objectives count (3-7)", totalValid == 120 ? "✅ PASS" : "⚠️  PARTIAL"),
            ("Req 2.4: Code examples (2+)", totalValid == 120 ? "✅ PASS" : "⚠️  PARTIAL"),
            ("Req 2.5: Exercises (3+)", totalValid == 120 ? "✅ PASS" : "⚠️  PARTIAL"),
            ("Req 8.2: Structure completeness", totalValid == 120 ? "✅ PASS" : "⚠️  PARTIAL"),
            ("Req 8.4: Content 1000-3000 words", totalValid == 120 ? "✅ PASS" : "⚠️  PARTIAL"),
            ("Req 9.1: All 120 lessons exist", totalLessons == 120 ? "✅ PASS" : "❌ FAIL"),
            ("Req 9.3: 100% pass validation", totalValid == 120 ? "✅ PASS" : "⚠️  PARTIAL"),
            ("Req 9.6: Sample lessons reviewed", "📋 Manual review required")
        };

        foreach (var (req, status) in reqChecks)
        {
            Console.WriteLine($"  {status,-20} {req}");
        }
    }

    private static void SaveResults(List<LevelValidationResult> results)
    {
        var jsonResults = new
        {
            checkpoint = "Phase 2",
            levels = "0-5",
            total_lessons = 120,
            validation_date = DateTime.UtcNow,
            results = results.Select(r => new
            {
                level = r.LevelNumber,
                title = r.LevelTitle,
                total_lessons = r.TotalLessons,
                valid_lessons = r.ValidCount,
                lessons_with_issues = r.IssueCount,
                lessons = r.LessonResults.Select(l => new
                {
                    lesson_number = l.LessonNumber,
                    title = l.Title,
                    is_valid = l.IsValid,
                    objectives = l.Objectives,
                    theory_sections = l.TheorySections,
                    total_words = l.TotalWords,
                    code_examples = l.CodeExamples,
                    exercises = l.Exercises,
                    has_summary = l.HasSummary,
                    issues = l.Issues
                })
            }),
            summary = new
            {
                total_valid = results.Sum(r => r.ValidCount),
                total_issues = results.Sum(r => r.IssueCount),
                total_theory_sections = results.Sum(r => r.LessonResults.Sum(l => l.TheorySections)),
                total_code_examples = results.Sum(r => r.LessonResults.Sum(l => l.CodeExamples)),
                total_exercises = results.Sum(r => r.LessonResults.Sum(l => l.Exercises))
            }
        };

        var jsonOutput = JsonSerializer.Serialize(jsonResults, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText("phase2_checkpoint_validation.json", jsonOutput);

        Console.WriteLine();
        Console.WriteLine("📄 Detailed results saved to: phase2_checkpoint_validation.json");
    }

    private static int CalculateTotalWords(LessonContent content)
    {
        int total = 0;

        foreach (var objective in content.Objectives)
        {
            total += CountWords(objective);
        }

        foreach (var theory in content.Theory)
        {
            total += CountWords(theory.Content);
        }

        foreach (var example in content.CodeExamples)
        {
            total += CountWords(example.Explanation);
        }

        foreach (var exercise in content.Exercises)
        {
            total += CountWords(exercise.Description);
        }

        total += CountWords(content.Summary);

        return total;
    }

    private static int CountWords(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return 0;

        return text.Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).Length;
    }
}

public class LevelValidationResult
{
    public int LevelNumber { get; set; }
    public string LevelTitle { get; set; }
    public int TotalLessons { get; set; }
    public int ValidCount { get; set; }
    public int IssueCount { get; set; }
    public List<LessonValidationResult> LessonResults { get; set; }
}

public class LessonValidationResult
{
    public int LessonNumber { get; set; }
    public string Title { get; set; }
    public bool IsValid { get; set; }
    public int Objectives { get; set; }
    public int TheorySections { get; set; }
    public int TotalWords { get; set; }
    public int CodeExamples { get; set; }
    public int Exercises { get; set; }
    public bool HasSummary { get; set; }
    public List<string> Issues { get; set; }
}
