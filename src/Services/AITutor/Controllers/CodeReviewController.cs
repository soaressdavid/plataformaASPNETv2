using AITutor.Service.Models;
using AITutor.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace AITutor.Service.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CodeReviewController : ControllerBase
{
    private readonly IAITutorService _aiTutorService;
    private readonly ILogger<CodeReviewController> _logger;

    public CodeReviewController(
        IAITutorService aiTutorService,
        ILogger<CodeReviewController> logger)
    {
        _aiTutorService = aiTutorService ?? throw new ArgumentNullException(nameof(aiTutorService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Reviews the provided code and returns AI-powered feedback.
    /// </summary>
    /// <param name="request">The code review request containing code and optional context</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Code review response with suggestions and analysis</returns>
    [HttpPost]
    [ProducesResponseType(typeof(CodeReviewResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<CodeReviewResponse>> ReviewCode(
        [FromBody] CodeReviewRequest request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Code))
        {
            return BadRequest(new { error = "Code cannot be null or empty" });
        }

        try
        {
            _logger.LogInformation("Received code review request");
            
            var response = await _aiTutorService.ReviewCodeAsync(
                request.Code, 
                request.Context, 
                cancellationToken);

            return Ok(response);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Invalid request for code review");
            return BadRequest(new { error = ex.Message });
        }
        catch (TimeoutException ex)
        {
            _logger.LogError(ex, "Code review request timed out");
            return StatusCode(StatusCodes.Status504GatewayTimeout, 
                new { error = "Code review request timed out. Please try again." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error during code review");
            return StatusCode(StatusCodes.Status500InternalServerError, 
                new { error = "An unexpected error occurred during code review" });
        }
    }
}
