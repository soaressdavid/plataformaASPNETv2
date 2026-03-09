using AITutor.Service.Models;

namespace AITutor.Service.Services;

public interface IGroqApiClient
{
    Task<GroqResponse> SendChatCompletionAsync(List<Message> messages, CancellationToken cancellationToken = default);
}
