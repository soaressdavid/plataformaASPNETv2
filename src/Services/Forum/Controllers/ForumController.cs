using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Data;
using Shared.Entities;

namespace Forum.Service.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ForumController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ForumController> _logger;

    public ForumController(ApplicationDbContext context, ILogger<ForumController> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Get all forum categories with thread counts
    /// </summary>
    [HttpGet("categories")]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategories()
    {
        var categories = await _context.ForumThreads
            .Where(t => !string.IsNullOrEmpty(t.Category))
            .GroupBy(t => t.Category)
            .Select(g => new CategoryDto
            {
                Name = g.Key!,
                ThreadCount = g.Count(),
                PostCount = g.Sum(t => t.Posts.Count),
                LastActivity = g.Max(t => t.UpdatedAt)
            })
            .ToListAsync();

        return Ok(categories);
    }

    /// <summary>
    /// Get threads for a specific category with pagination
    /// </summary>
    [HttpGet("categories/{category}/threads")]
    public async Task<ActionResult<PagedResult<ThreadListDto>>> GetCategoryThreads(
        string category,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        var query = _context.ForumThreads
            .Where(t => t.Category == category)
            .Include(t => t.Author)
            .Include(t => t.Posts)
            .OrderByDescending(t => t.IsPinned)
            .ThenByDescending(t => t.UpdatedAt);

        var total = await query.CountAsync();
        var threads = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(t => new ThreadListDto
            {
                Id = t.Id,
                Title = t.Title,
                AuthorName = t.Author.Name,
                AuthorId = t.AuthorId,
                PostCount = t.Posts.Count,
                ViewCount = t.ViewCount,
                IsPinned = t.IsPinned,
                IsLocked = t.IsLocked,
                HasAcceptedAnswer = t.AcceptedAnswerId != null,
                CreatedAt = t.CreatedAt,
                UpdatedAt = t.UpdatedAt
            })
            .ToListAsync();

        return Ok(new PagedResult<ThreadListDto>
        {
            Items = threads,
            TotalCount = total,
            Page = page,
            PageSize = pageSize
        });
    }

    /// <summary>
    /// Get threads for a specific challenge
    /// </summary>
    [HttpGet("challenges/{challengeId}/threads")]
    public async Task<ActionResult<IEnumerable<ThreadListDto>>> GetChallengeThreads(Guid challengeId)
    {
        var threads = await _context.ForumThreads
            .Where(t => t.ChallengeId == challengeId)
            .Include(t => t.Author)
            .Include(t => t.Posts)
            .OrderByDescending(t => t.IsPinned)
            .ThenByDescending(t => t.UpdatedAt)
            .Select(t => new ThreadListDto
            {
                Id = t.Id,
                Title = t.Title,
                AuthorName = t.Author.Name,
                AuthorId = t.AuthorId,
                PostCount = t.Posts.Count,
                ViewCount = t.ViewCount,
                IsPinned = t.IsPinned,
                IsLocked = t.IsLocked,
                HasAcceptedAnswer = t.AcceptedAnswerId != null,
                CreatedAt = t.CreatedAt,
                UpdatedAt = t.UpdatedAt
            })
            .ToListAsync();

        return Ok(threads);
    }

    /// <summary>
    /// Get threads for a specific lesson
    /// </summary>
    [HttpGet("lessons/{lessonId}/threads")]
    public async Task<ActionResult<IEnumerable<ThreadListDto>>> GetLessonThreads(Guid lessonId)
    {
        var threads = await _context.ForumThreads
            .Where(t => t.LessonId == lessonId)
            .Include(t => t.Author)
            .Include(t => t.Posts)
            .OrderByDescending(t => t.IsPinned)
            .ThenByDescending(t => t.UpdatedAt)
            .Select(t => new ThreadListDto
            {
                Id = t.Id,
                Title = t.Title,
                AuthorName = t.Author.Name,
                AuthorId = t.AuthorId,
                PostCount = t.Posts.Count,
                ViewCount = t.ViewCount,
                IsPinned = t.IsPinned,
                IsLocked = t.IsLocked,
                HasAcceptedAnswer = t.AcceptedAnswerId != null,
                CreatedAt = t.CreatedAt,
                UpdatedAt = t.UpdatedAt
            })
            .ToListAsync();

        return Ok(threads);
    }

    /// <summary>
    /// Get a specific thread with all posts
    /// </summary>
    [HttpGet("threads/{threadId}")]
    public async Task<ActionResult<ThreadDetailDto>> GetThread(Guid threadId)
    {
        var thread = await _context.ForumThreads
            .Include(t => t.Author)
            .ThenInclude(a => a.Progress)
            .Include(t => t.Posts)
            .ThenInclude(p => p.Author)
            .ThenInclude(a => a.Progress)
            .Include(t => t.Challenge)
            .Include(t => t.Lesson)
            .FirstOrDefaultAsync(t => t.Id == threadId);

        if (thread == null)
        {
            return NotFound();
        }

        // Increment view count
        thread.ViewCount++;
        await _context.SaveChangesAsync();

        var result = new ThreadDetailDto
        {
            Id = thread.Id,
            Title = thread.Title,
            Content = thread.Content,
            AuthorId = thread.AuthorId,
            AuthorName = thread.Author.Name,
            AuthorLevel = thread.Author.Progress?.Level ?? 0,
            AuthorXP = thread.Author.Progress?.TotalXP ?? 0,
            ChallengeId = thread.ChallengeId,
            ChallengeName = thread.Challenge?.Title,
            LessonId = thread.LessonId,
            LessonName = thread.Lesson?.Title,
            Category = thread.Category,
            ViewCount = thread.ViewCount,
            IsPinned = thread.IsPinned,
            IsLocked = thread.IsLocked,
            AcceptedAnswerId = thread.AcceptedAnswerId,
            CreatedAt = thread.CreatedAt,
            UpdatedAt = thread.UpdatedAt,
            Posts = thread.Posts
                .OrderBy(p => p.CreatedAt)
                .Select(p => new PostDto
                {
                    Id = p.Id,
                    Content = p.Content,
                    AuthorId = p.AuthorId,
                    AuthorName = p.Author.Name,
                    AuthorLevel = p.Author.Progress?.Level ?? 0,
                    AuthorXP = p.Author.Progress?.TotalXP ?? 0,
                    Upvotes = p.Upvotes,
                    IsEdited = p.IsEdited,
                    LastEditedAt = p.LastEditedAt,
                    IsAcceptedAnswer = p.IsAcceptedAnswer,
                    CreatedAt = p.CreatedAt
                })
                .ToList()
        };

        return Ok(result);
    }

    /// <summary>
    /// Create a new thread
    /// </summary>
    [HttpPost("threads")]
    public async Task<ActionResult<ThreadDetailDto>> CreateThread([FromBody] CreateThreadRequest request)
    {
        // Validate user exists
        var user = await _context.Users.FindAsync(request.AuthorId);
        if (user == null)
        {
            return BadRequest("User not found");
        }

        var thread = new ForumThread
        {
            Title = request.Title,
            Content = request.Content,
            AuthorId = request.AuthorId,
            ChallengeId = request.ChallengeId,
            LessonId = request.LessonId,
            Category = request.Category
        };

        _context.ForumThreads.Add(thread);
        await _context.SaveChangesAsync();

        // Reload with navigation properties
        await _context.Entry(thread)
            .Reference(t => t.Author)
            .LoadAsync();
        await _context.Entry(thread.Author)
            .Reference(a => a.Progress)
            .LoadAsync();

        var result = new ThreadDetailDto
        {
            Id = thread.Id,
            Title = thread.Title,
            Content = thread.Content,
            AuthorId = thread.AuthorId,
            AuthorName = thread.Author.Name,
            AuthorLevel = thread.Author.Progress?.Level ?? 0,
            AuthorXP = thread.Author.Progress?.TotalXP ?? 0,
            ChallengeId = thread.ChallengeId,
            LessonId = thread.LessonId,
            Category = thread.Category,
            ViewCount = thread.ViewCount,
            IsPinned = thread.IsPinned,
            IsLocked = thread.IsLocked,
            CreatedAt = thread.CreatedAt,
            UpdatedAt = thread.UpdatedAt,
            Posts = new List<PostDto>()
        };

        return CreatedAtAction(nameof(GetThread), new { threadId = thread.Id }, result);
    }

    /// <summary>
    /// Create a reply/post in a thread
    /// </summary>
    [HttpPost("threads/{threadId}/posts")]
    public async Task<ActionResult<PostDto>> CreatePost(Guid threadId, [FromBody] CreatePostRequest request)
    {
        var thread = await _context.ForumThreads
            .Include(t => t.Posts)
            .FirstOrDefaultAsync(t => t.Id == threadId);

        if (thread == null)
        {
            return NotFound("Thread not found");
        }

        if (thread.IsLocked)
        {
            return BadRequest("Thread is locked");
        }

        // Validate user exists
        var user = await _context.Users
            .Include(u => u.Progress)
            .FirstOrDefaultAsync(u => u.Id == request.AuthorId);
        
        if (user == null)
        {
            return BadRequest("User not found");
        }

        var post = new ForumPost
        {
            ThreadId = threadId,
            AuthorId = request.AuthorId,
            Content = request.Content
        };

        _context.ForumPosts.Add(post);
        
        // Update thread's UpdatedAt timestamp
        thread.UpdatedAt = DateTime.UtcNow;
        
        await _context.SaveChangesAsync();

        var result = new PostDto
        {
            Id = post.Id,
            Content = post.Content,
            AuthorId = post.AuthorId,
            AuthorName = user.Name,
            AuthorLevel = user.Progress?.Level ?? 0,
            AuthorXP = user.Progress?.TotalXP ?? 0,
            Upvotes = post.Upvotes,
            IsEdited = post.IsEdited,
            IsAcceptedAnswer = post.IsAcceptedAnswer,
            CreatedAt = post.CreatedAt
        };

        return CreatedAtAction(nameof(GetThread), new { threadId }, result);
    }

    /// <summary>
    /// Upvote a post
    /// </summary>
    [HttpPost("posts/{postId}/upvote")]
    public async Task<ActionResult> UpvotePost(Guid postId, [FromBody] VoteRequest request)
    {
        var post = await _context.ForumPosts
            .Include(p => p.Votes)
            .FirstOrDefaultAsync(p => p.Id == postId);

        if (post == null)
        {
            return NotFound("Post not found");
        }

        // Check if user already voted
        var existingVote = await _context.ForumPostVotes
            .FirstOrDefaultAsync(v => v.PostId == postId && v.UserId == request.UserId);

        if (existingVote != null)
        {
            // Remove vote if clicking again
            _context.ForumPostVotes.Remove(existingVote);
            post.Upvotes--;
        }
        else
        {
            // Add new vote
            var vote = new ForumPostVote
            {
                PostId = postId,
                UserId = request.UserId,
                VoteValue = 1
            };
            _context.ForumPostVotes.Add(vote);
            post.Upvotes++;

            // Award XP if post reaches 5 upvotes
            if (post.Upvotes == 5)
            {
                var author = await _context.Users
                    .Include(u => u.Progress)
                    .FirstOrDefaultAsync(u => u.Id == post.AuthorId);

                if (author?.Progress != null)
                {
                    author.Progress.TotalXP += 10;
                    _logger.LogInformation("Awarded 10 XP to user {UserId} for helpful forum post", author.Id);
                }
            }
        }

        await _context.SaveChangesAsync();

        return Ok(new { upvotes = post.Upvotes });
    }

    /// <summary>
    /// Mark a post as the accepted answer
    /// </summary>
    [HttpPost("threads/{threadId}/accept/{postId}")]
    public async Task<ActionResult> AcceptAnswer(Guid threadId, Guid postId, [FromBody] AcceptAnswerRequest request)
    {
        var thread = await _context.ForumThreads
            .Include(t => t.Posts)
            .FirstOrDefaultAsync(t => t.Id == threadId);

        if (thread == null)
        {
            return NotFound("Thread not found");
        }

        // Only thread author can accept answers
        if (thread.AuthorId != request.UserId)
        {
            return Forbid();
        }

        var post = thread.Posts.FirstOrDefault(p => p.Id == postId);
        if (post == null)
        {
            return NotFound("Post not found");
        }

        // Remove previous accepted answer if any
        if (thread.AcceptedAnswerId.HasValue)
        {
            var previousAnswer = thread.Posts.FirstOrDefault(p => p.Id == thread.AcceptedAnswerId);
            if (previousAnswer != null)
            {
                previousAnswer.IsAcceptedAnswer = false;
            }
        }

        // Set new accepted answer
        thread.AcceptedAnswerId = postId;
        post.IsAcceptedAnswer = true;

        await _context.SaveChangesAsync();

        return Ok();
    }

    /// <summary>
    /// Edit a post
    /// </summary>
    [HttpPut("posts/{postId}")]
    public async Task<ActionResult> EditPost(Guid postId, [FromBody] EditPostRequest request)
    {
        var post = await _context.ForumPosts.FindAsync(postId);

        if (post == null)
        {
            return NotFound("Post not found");
        }

        // Only post author can edit
        if (post.AuthorId != request.UserId)
        {
            return Forbid();
        }

        post.Content = request.Content;
        post.IsEdited = true;
        post.LastEditedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return Ok();
    }

    /// <summary>
    /// Report a post
    /// </summary>
    [HttpPost("posts/{postId}/report")]
    public async Task<ActionResult> ReportPost(Guid postId, [FromBody] ReportRequest request)
    {
        var post = await _context.ForumPosts.FindAsync(postId);

        if (post == null)
        {
            return NotFound("Post not found");
        }

        post.IsReported = true;
        await _context.SaveChangesAsync();

        _logger.LogWarning("Post {PostId} reported by user {UserId}: {Reason}", 
            postId, request.UserId, request.Reason);

        return Ok();
    }
}

// DTOs
public record CategoryDto
{
    public string Name { get; init; } = string.Empty;
    public int ThreadCount { get; init; }
    public int PostCount { get; init; }
    public DateTime LastActivity { get; init; }
}

public record ThreadListDto
{
    public Guid Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public string AuthorName { get; init; } = string.Empty;
    public Guid AuthorId { get; init; }
    public int PostCount { get; init; }
    public int ViewCount { get; init; }
    public bool IsPinned { get; init; }
    public bool IsLocked { get; init; }
    public bool HasAcceptedAnswer { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
}

public record ThreadDetailDto
{
    public Guid Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Content { get; init; } = string.Empty;
    public Guid AuthorId { get; init; }
    public string AuthorName { get; init; } = string.Empty;
    public int AuthorLevel { get; init; }
    public int AuthorXP { get; init; }
    public Guid? ChallengeId { get; init; }
    public string? ChallengeName { get; init; }
    public Guid? LessonId { get; init; }
    public string? LessonName { get; init; }
    public string? Category { get; init; }
    public int ViewCount { get; init; }
    public bool IsPinned { get; init; }
    public bool IsLocked { get; init; }
    public Guid? AcceptedAnswerId { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
    public List<PostDto> Posts { get; init; } = new();
}

public record PostDto
{
    public Guid Id { get; init; }
    public string Content { get; init; } = string.Empty;
    public Guid AuthorId { get; init; }
    public string AuthorName { get; init; } = string.Empty;
    public int AuthorLevel { get; init; }
    public int AuthorXP { get; init; }
    public int Upvotes { get; init; }
    public bool IsEdited { get; init; }
    public DateTime? LastEditedAt { get; init; }
    public bool IsAcceptedAnswer { get; init; }
    public DateTime CreatedAt { get; init; }
}

public record CreateThreadRequest
{
    public string Title { get; init; } = string.Empty;
    public string Content { get; init; } = string.Empty;
    public Guid AuthorId { get; init; }
    public Guid? ChallengeId { get; init; }
    public Guid? LessonId { get; init; }
    public string? Category { get; init; }
}

public record CreatePostRequest
{
    public string Content { get; init; } = string.Empty;
    public Guid AuthorId { get; init; }
}

public record VoteRequest
{
    public Guid UserId { get; init; }
}

public record AcceptAnswerRequest
{
    public Guid UserId { get; init; }
}

public record EditPostRequest
{
    public Guid UserId { get; init; }
    public string Content { get; init; } = string.Empty;
}

public record ReportRequest
{
    public Guid UserId { get; init; }
    public string Reason { get; init; } = string.Empty;
}

public record PagedResult<T>
{
    public List<T> Items { get; init; } = new();
    public int TotalCount { get; init; }
    public int Page { get; init; }
    public int PageSize { get; init; }
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
}
