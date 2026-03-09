using AITutor.Service.Models;

namespace AITutor.Service.Services;

public interface IAITutorService
{
    Task<CodeReviewResponse> ReviewCodeAsync(string code, string? context = null, CancellationToken cancellationToken = default);
}
