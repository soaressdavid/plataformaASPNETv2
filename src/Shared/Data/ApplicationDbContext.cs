using Microsoft.EntityFrameworkCore;
using Shared.Entities;

namespace Shared.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Course> Courses { get; set; } = null!;
    public DbSet<Lesson> Lessons { get; set; } = null!;
    public DbSet<Challenge> Challenges { get; set; } = null!;
    public DbSet<TestCase> TestCases { get; set; } = null!;
    public DbSet<Submission> Submissions { get; set; } = null!;
    public DbSet<Enrollment> Enrollments { get; set; } = null!;
    public DbSet<Progress> Progresses { get; set; } = null!;
    public DbSet<LessonCompletion> LessonCompletions { get; set; } = null!;
    public DbSet<Project> Projects { get; set; } = null!;
    public DbSet<ProjectProgress> ProjectProgresses { get; set; } = null!;
    public DbSet<CurriculumLevel> CurriculumLevels { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User configuration
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Email).IsUnique();
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
            entity.Property(e => e.PasswordHash).IsRequired();

            entity.HasOne(e => e.Progress)
                .WithOne(p => p.User)
                .HasForeignKey<Progress>(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(e => e.Enrollments)
                .WithOne(e => e.User)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(e => e.Submissions)
                .WithOne(s => s.User)
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(e => e.LessonCompletions)
                .WithOne(lc => lc.User)
                .HasForeignKey(lc => lc.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(e => e.ProjectProgresses)
                .WithOne(pp => pp.User)
                .HasForeignKey(pp => pp.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Course configuration
        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedNever(); // Don't auto-generate IDs
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).IsRequired();
            entity.Property(e => e.Level).IsRequired();

            entity.HasOne(e => e.CurriculumLevel)
                .WithMany(l => l.Courses)
                .HasForeignKey(e => e.LevelId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasMany(e => e.Lessons)
                .WithOne(l => l.Course)
                .HasForeignKey(l => l.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(e => e.Enrollments)
                .WithOne(e => e.Course)
                .HasForeignKey(e => e.CourseId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Lesson configuration
        modelBuilder.Entity<Lesson>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedNever(); // Don't auto-generate IDs
            entity.HasIndex(e => new { e.CourseId, e.OrderIndex });
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Content).IsRequired();

            entity.HasMany(e => e.Completions)
                .WithOne(lc => lc.Lesson)
                .HasForeignKey(lc => lc.LessonId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Challenge configuration
        modelBuilder.Entity<Challenge>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).IsRequired();
            entity.Property(e => e.Difficulty).IsRequired();
            entity.Property(e => e.StarterCode).IsRequired();

            entity.HasMany(e => e.TestCases)
                .WithOne(tc => tc.Challenge)
                .HasForeignKey(tc => tc.ChallengeId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(e => e.Submissions)
                .WithOne(s => s.Challenge)
                .HasForeignKey(s => s.ChallengeId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // TestCase configuration
        modelBuilder.Entity<TestCase>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.ChallengeId, e.OrderIndex });
            entity.Property(e => e.Input).IsRequired();
            entity.Property(e => e.ExpectedOutput).IsRequired();
        });

        // Submission configuration
        modelBuilder.Entity<Submission>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.UserId, e.ChallengeId });
            entity.Property(e => e.Code).IsRequired();
        });

        // Enrollment configuration
        modelBuilder.Entity<Enrollment>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.UserId, e.CourseId }).IsUnique();
        });

        // Progress configuration
        modelBuilder.Entity<Progress>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.UserId).IsUnique();
        });

        // LessonCompletion configuration
        modelBuilder.Entity<LessonCompletion>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.UserId, e.LessonId }).IsUnique();
        });

        // Project configuration
        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).IsRequired();
            entity.Property(e => e.Steps).IsRequired();
            entity.Property(e => e.XPReward).IsRequired();

            entity.HasMany<ProjectProgress>()
                .WithOne(pp => pp.Project)
                .HasForeignKey(pp => pp.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // ProjectProgress configuration
        modelBuilder.Entity<ProjectProgress>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.UserId, e.ProjectId }).IsUnique();
            entity.Property(e => e.CurrentStep).IsRequired();
            entity.Property(e => e.CompletedSteps).IsRequired();
        });

        // CurriculumLevel configuration
        modelBuilder.Entity<CurriculumLevel>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedNever(); // Don't auto-generate IDs
            entity.HasIndex(e => e.Number);
            entity.Property(e => e.Number).IsRequired();
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).IsRequired();
            entity.Property(e => e.RequiredXP).IsRequired();

            entity.HasMany(e => e.Courses)
                .WithOne(c => c.CurriculumLevel)
                .HasForeignKey(c => c.LevelId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(e => e.Project)
                .WithOne(p => p.CurriculumLevel)
                .HasForeignKey<Project>(p => p.LevelId)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}
