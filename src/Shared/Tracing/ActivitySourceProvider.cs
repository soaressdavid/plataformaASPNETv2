using System.Diagnostics;

namespace Shared.Tracing;

/// <summary>
/// Provedor centralizado de ActivitySource para criar spans customizados
/// </summary>
public static class ActivitySourceProvider
{
    private static readonly Dictionary<string, ActivitySource> _sources = new();
    private static readonly object _lock = new();

    /// <summary>
    /// Obtém ou cria um ActivitySource para o serviço especificado
    /// </summary>
    public static ActivitySource GetSource(string serviceName)
    {
        if (_sources.TryGetValue(serviceName, out var source))
        {
            return source;
        }

        lock (_lock)
        {
            if (_sources.TryGetValue(serviceName, out source))
            {
                return source;
            }

            source = new ActivitySource(serviceName, "1.0.0");
            _sources[serviceName] = source;
            return source;
        }
    }

    /// <summary>
    /// Cria um span customizado para uma operação
    /// </summary>
    public static Activity? StartActivity(
        string serviceName,
        string operationName,
        ActivityKind kind = ActivityKind.Internal)
    {
        var source = GetSource(serviceName);
        return source.StartActivity(operationName, kind);
    }

    /// <summary>
    /// Adiciona tags a um span ativo
    /// </summary>
    public static void AddTag(string key, object? value)
    {
        Activity.Current?.SetTag(key, value);
    }

    /// <summary>
    /// Adiciona evento a um span ativo
    /// </summary>
    public static void AddEvent(string name, params (string key, object? value)[] tags)
    {
        var activity = Activity.Current;
        if (activity == null) return;

        var tagsDict = new ActivityTagsCollection();
        foreach (var (key, value) in tags)
        {
            tagsDict.Add(key, value);
        }
        
        activity.AddEvent(new ActivityEvent(name, tags: tagsDict));
    }

    /// <summary>
    /// Registra uma exceção no span ativo
    /// </summary>
    public static void RecordException(Exception exception)
    {
        var activity = Activity.Current;
        if (activity == null) return;

        activity.SetStatus(ActivityStatusCode.Error, exception.Message);
        
        // Adicionar exceção como evento
        var tags = new ActivityTagsCollection
        {
            { "exception.type", exception.GetType().FullName },
            { "exception.message", exception.Message },
            { "exception.stacktrace", exception.StackTrace }
        };
        
        activity.AddEvent(new ActivityEvent("exception", tags: tags));
    }
}
