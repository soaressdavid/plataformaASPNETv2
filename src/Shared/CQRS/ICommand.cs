using MediatR;

namespace Shared.CQRS;

/// <summary>
/// Marker interface para commands (sem retorno)
/// </summary>
public interface ICommand : IRequest
{
}

/// <summary>
/// Interface para commands com retorno
/// </summary>
public interface ICommand<out TResponse> : IRequest<TResponse>
{
}
