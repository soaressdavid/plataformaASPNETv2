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
    public DbSet<Models.IdeSession> IdeSessions { get; set; } = null!;
    public DbSet<ForumThread> ForumThreads { get; set; } = null!;
    public DbSet<ForumPost> ForumPosts { get; set; } = null!;
    public DbSet<ForumPostVote> ForumPostVotes { get; set; } = null!;
    public DbSet<ChatRoom> ChatRooms { get; set; } = null!;
    public DbSet<ChatMessage> ChatMessages { get; set; } = null!;
    public DbSet<ChatRoomMember> ChatRoomMembers { get; set; } = null!;
    public DbSet<CollaborativeSession> CollaborativeSessions { get; set; } = null!;
    public DbSet<CollaborativeSessionParticipant> CollaborativeSessionParticipants { get; set; } = null!;

    public override int SaveChanges()
    {
        HandleSoftDelete();
        UpdateAuditFields();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        HandleSoftDelete();
        UpdateAuditFields();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void HandleSoftDelete()
    {
        var deletedEntries = ChangeTracker.Entries()
            .Where(e => e.Entity is Models.BaseEntity && e.State == EntityState.Deleted);

        foreach (var entry in deletedEntries)
        {
            var entity = (Models.BaseEntity)entry.Entity;
            
            // Check if this is a hard delete
            if (_hardDeleteIds.Contains(entity.Id))
            {
                // Skip soft delete for hard deletes
                _hardDeleteIds.Remove(entity.Id);
                continue;
            }
            
            // Convert physical delete to soft delete
            entry.State = EntityState.Modified;
            entity.IsDeleted = true;
            entity.UpdatedAt = DateTime.UtcNow;
        }
    }

    private void UpdateAuditFields()
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.Entity is Models.BaseEntity && 
                       (e.State == EntityState.Added || e.State == EntityState.Modified));

        foreach (var entry in entries)
        {
            var entity = (Models.BaseEntity)entry.Entity;
            
            if (entry.State == EntityState.Added)
            {
                entity.CreatedAt = DateTime.UtcNow;
            }
            
            entity.UpdatedAt = DateTime.UtcNow;
        }
    }

    /// <summary>
    /// Gets a queryable that includes soft-deleted entities.
    /// Use this when you need to query soft-deleted records.
    /// </summary>
    public IQueryable<T> GetIncludingDeleted<T>() where T : Models.BaseEntity
    {
        return Set<T>().IgnoreQueryFilters();
    }

    /// <summary>
    /// Gets only soft-deleted entities.
    /// </summary>
    public IQueryable<T> GetOnlyDeleted<T>() where T : Models.BaseEntity
    {
        return Set<T>().IgnoreQueryFilters().Where(e => e.IsDeleted);
    }

    /// <summary>
    /// Restores a soft-deleted entity by setting IsDeleted to false.
    /// </summary>
    public void Restore<T>(T entity) where T : Models.BaseEntity
    {
        entity.IsDeleted = false;
        entity.UpdatedAt = DateTime.UtcNow;
        Entry(entity).State = EntityState.Modified;
    }

    /// <summary>
    /// Permanently deletes an entity from the database.
    /// This bypasses the soft delete mechanism and physically removes the record.
    /// Use with caution!
    /// </summary>
    public void HardDelete<T>(T entity) where T : Models.BaseEntity
    {
        // Mark entity for hard delete by setting a flag
        Entry(entity).State = EntityState.Deleted;
        
        // We need to bypass the soft delete in SaveChanges
        // Store the entity ID to check in HandleSoftDelete
        if (!_hardDeleteIds.Contains(entity.Id))
        {
            _hardDeleteIds.Add(entity.Id);
        }
    }

    private readonly HashSet<Guid> _hardDeleteIds = new();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure global query filter for soft delete
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(Models.BaseEntity).IsAssignableFrom(entityType.ClrType))
            {
                var parameter = System.Linq.Expressions.Expression.Parameter(entityType.ClrType, "e");
                var property = System.Linq.Expressions.Expression.Property(parameter, nameof(Models.BaseEntity.IsDeleted));
                var filter = System.Linq.Expressions.Expression.Lambda(
                    System.Linq.Expressions.Expression.Equal(property, System.Linq.Expressions.Expression.Constant(false)),
                    parameter);
                modelBuilder.Entity(entityType.ClrType).HasQueryFilter(filter);
            }
        }

        // User configuration
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Email).IsUnique().HasFilter("[IsDeleted] = 0");
            entity.HasIndex(e => e.Name).HasFilter("[IsDeleted] = 0");
            entity.HasIndex(e => e.CreatedAt);
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
            entity.Property(e => e.Id).ValueGeneratedNever();
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
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.HasIndex(e => new { e.CourseId, e.OrderIndex });
            entity.HasIndex(e => e.CreatedAt).HasFilter("[IsDeleted] = 0");
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
            entity.HasIndex(e => e.Difficulty).HasFilter("[IsDeleted] = 0");
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
            entity.HasIndex(e => e.CreatedAt);
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
            entity.HasIndex(e => new { e.UserId, e.CreatedAt });
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
            entity.Property(e => e.Id).ValueGeneratedNever();
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

        // IdeSession configuration
        modelBuilder.Entity<Models.IdeSession>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.UserId).IsUnique();
            entity.Property(e => e.UserId).IsRequired().HasMaxLength(450);
            entity.Property(e => e.SessionData).IsRequired();
            entity.Property(e => e.LastSavedAt).IsRequired();
        });

        // ForumThread configuration
        modelBuilder.Entity<ForumThread>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.AuthorId);
            entity.HasIndex(e => e.ChallengeId);
            entity.HasIndex(e => e.LessonId);
            entity.HasIndex(e => e.Category);
            entity.HasIndex(e => e.CreatedAt);
            entity.HasIndex(e => new { e.IsPinned, e.CreatedAt });
            entity.Property(e => e.Title).IsRequired().HasMaxLength(300);
            entity.Property(e => e.Content).IsRequired();

            entity.HasOne(e => e.Author)
                .WithMany()
                .HasForeignKey(e => e.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Challenge)
                .WithMany()
                .HasForeignKey(e => e.ChallengeId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(e => e.Lesson)
                .WithMany()
                .HasForeignKey(e => e.LessonId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasMany(e => e.Posts)
                .WithOne(p => p.Thread)
                .HasForeignKey(p => p.ThreadId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // ForumPost configuration
        modelBuilder.Entity<ForumPost>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.ThreadId);
            entity.HasIndex(e => e.AuthorId);
            entity.HasIndex(e => e.CreatedAt);
            entity.HasIndex(e => new { e.ThreadId, e.Upvotes });
            entity.Property(e => e.Content).IsRequired();

            entity.HasOne(e => e.Author)
                .WithMany()
                .HasForeignKey(e => e.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(e => e.Votes)
                .WithOne(v => v.Post)
                .HasForeignKey(v => v.PostId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // ForumPostVote configuration
        modelBuilder.Entity<ForumPostVote>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.PostId, e.UserId }).IsUnique();
            entity.Property(e => e.VoteValue).IsRequired();

            entity.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // ChatRoom configuration
        modelBuilder.Entity<ChatRoom>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Type);
            entity.HasIndex(e => e.CourseId);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);

            entity.HasMany(e => e.Messages)
                .WithOne(m => m.ChatRoom)
                .HasForeignKey(m => m.ChatRoomId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(e => e.Members)
                .WithOne(m => m.ChatRoom)
                .HasForeignKey(m => m.ChatRoomId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // ChatMessage configuration
        modelBuilder.Entity<ChatMessage>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.ChatRoomId);
            entity.HasIndex(e => e.UserId);
            entity.HasIndex(e => e.CreatedAt);
            entity.Property(e => e.Content).IsRequired().HasMaxLength(2000);

            entity.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // ChatRoomMember configuration
        modelBuilder.Entity<ChatRoomMember>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.ChatRoomId, e.UserId }).IsUnique();
            entity.HasIndex(e => e.UserId);

            entity.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // CollaborativeSession configuration
        modelBuilder.Entity<CollaborativeSession>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.CreatedByUserId);
            entity.HasIndex(e => e.ChallengeId);
            entity.HasIndex(e => new { e.Status, e.CreatedAt });
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Code).IsRequired();
            entity.Property(e => e.Language).IsRequired().HasMaxLength(50);

            entity.HasOne(e => e.CreatedByUser)
                .WithMany()
                .HasForeignKey(e => e.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Challenge)
                .WithMany()
                .HasForeignKey(e => e.ChallengeId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasMany(e => e.Participants)
                .WithOne(p => p.Session)
                .HasForeignKey(p => p.SessionId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // CollaborativeSessionParticipant configuration
        modelBuilder.Entity<CollaborativeSessionParticipant>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.SessionId);
            entity.HasIndex(e => e.UserId);
            entity.HasIndex(e => new { e.SessionId, e.UserId }).IsUnique();
            entity.HasIndex(e => e.IsActive);

            entity.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
