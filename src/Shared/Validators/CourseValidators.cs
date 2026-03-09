using FluentValidation;

namespace Shared.Validators;

/// <summary>
/// Validator for course creation/update requests
/// </summary>
public class CreateCourseRequestValidator : AbstractValidator<object>
{
    public CreateCourseRequestValidator()
    {
        // Title validation
        RuleFor(x => GetProperty<string>(x, "Title"))
            .NotEmpty().WithMessage("Title is required")
            .MinimumLength(3).WithMessage("Title must be at least 3 characters")
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters");

        // Description validation
        RuleFor(x => GetProperty<string>(x, "Description"))
            .NotEmpty().WithMessage("Description is required")
            .MinimumLength(10).WithMessage("Description must be at least 10 characters")
            .MaximumLength(2000).WithMessage("Description must not exceed 2000 characters");

        // Level ID validation
        RuleFor(x => GetProperty<Guid>(x, "LevelId"))
            .NotEmpty().WithMessage("LevelId is required");

        // Order validation
        RuleFor(x => GetProperty<int>(x, "Order"))
            .GreaterThanOrEqualTo(0).WithMessage("Order must be non-negative");
    }

    private static T GetProperty<T>(object obj, string propertyName)
    {
        var property = obj.GetType().GetProperty(propertyName);
        return property != null ? (T)property.GetValue(obj)! : default!;
    }
}

/// <summary>
/// Validator for lesson creation/update requests
/// </summary>
public class CreateLessonRequestValidator : AbstractValidator<object>
{
    public CreateLessonRequestValidator()
    {
        // Title validation
        RuleFor(x => GetProperty<string>(x, "Title"))
            .NotEmpty().WithMessage("Title is required")
            .MinimumLength(3).WithMessage("Title must be at least 3 characters")
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters");

        // Description validation
        RuleFor(x => GetProperty<string>(x, "Description"))
            .NotEmpty().WithMessage("Description is required")
            .MinimumLength(10).WithMessage("Description must be at least 10 characters")
            .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters");

        // Course ID validation
        RuleFor(x => GetProperty<Guid>(x, "CourseId"))
            .NotEmpty().WithMessage("CourseId is required");

        // Order validation
        RuleFor(x => GetProperty<int>(x, "Order"))
            .GreaterThanOrEqualTo(0).WithMessage("Order must be non-negative");

        // Difficulty validation
        RuleFor(x => GetProperty<string>(x, "Difficulty"))
            .NotEmpty().WithMessage("Difficulty is required")
            .Must(d => new[] { "Beginner", "Intermediate", "Advanced" }.Contains(d))
            .WithMessage("Difficulty must be Beginner, Intermediate, or Advanced");
    }

    private static T GetProperty<T>(object obj, string propertyName)
    {
        var property = obj.GetType().GetProperty(propertyName);
        return property != null ? (T)property.GetValue(obj)! : default!;
    }
}

/// <summary>
/// Validator for level creation/update requests
/// </summary>
public class CreateLevelRequestValidator : AbstractValidator<object>
{
    public CreateLevelRequestValidator()
    {
        // Number validation
        RuleFor(x => GetProperty<int>(x, "Number"))
            .GreaterThan(0).WithMessage("Number must be greater than 0")
            .LessThanOrEqualTo(20).WithMessage("Number must not exceed 20");

        // Title validation
        RuleFor(x => GetProperty<string>(x, "Title"))
            .NotEmpty().WithMessage("Title is required")
            .MinimumLength(3).WithMessage("Title must be at least 3 characters")
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters");

        // Description validation
        RuleFor(x => GetProperty<string>(x, "Description"))
            .NotEmpty().WithMessage("Description is required")
            .MinimumLength(10).WithMessage("Description must be at least 10 characters")
            .MaximumLength(2000).WithMessage("Description must not exceed 2000 characters");
    }

    private static T GetProperty<T>(object obj, string propertyName)
    {
        var property = obj.GetType().GetProperty(propertyName);
        return property != null ? (T)property.GetValue(obj)! : default!;
    }
}
