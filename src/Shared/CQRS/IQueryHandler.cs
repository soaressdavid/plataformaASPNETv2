using MediatR;

namespace Shared.CQRS;

/// <summary>
/// Handler para queries
/// </summary>
public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, TResponse>
    where TQuery : IQuery<TResponse>
{
}
