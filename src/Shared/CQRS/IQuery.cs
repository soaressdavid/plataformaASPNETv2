using MediatR;

namespace Shared.CQRS;

/// <summary>
/// Interface para queries (sempre retornam dados)
/// </summary>
public interface IQuery<out TResponse> : IRequest<TResponse>
{
}
