# OpenTelemetry Distributed Tracing Guide

## 🎯 Objetivo

Implementar distributed tracing com OpenTelemetry e Jaeger para rastrear requisições através de múltiplos serviços.

---

## 📋 O que é Distributed Tracing?

Distributed tracing permite rastrear uma requisição através de múltiplos serviços, criando uma visão completa do fluxo de execução. Cada operação cria um "span" que contém:

- **Trace ID**: Identificador único da requisição completa
- **Span ID**: Identificador único da operação específica
- **Parent Span ID**: ID do span pai (para criar hierarquia)
- **Duration**: Tempo de execução
- **Tags**: Metadados customizados
- **Events**: Eventos que ocorreram durante a execução
- **Status**: Sucesso ou erro

---

## 🚀 Quick Start

### 1. Iniciar Jaeger

```bash
cd monitoring
docker-compose -f docker-compose.monitoring.yml up -d jaeger
```

Acesse a UI do Jaeger: http://localhost:16686

### 2. Configurar Serviço

No `Program.cs` de cada serviço:

```csharp
using Shared.Tracing;

var builder = WebApplication.CreateBuilder(args);

// Adicionar distributed tracing
builder.Services.AddDistributedTracing(
    serviceName: "CourseService",
    jaegerEndpoint: builder.Configuration["Jaeger:AgentHost"] ?? "localhost"
);

// ... resto da configuração
```

### 3. Configurar appsettings.json

```json
{
  "Jaeger": {
    "AgentHost": "localhost",
    "AgentPort": 6831
  }
}
```

### 4. Criar Spans Customizados

```csharp
using Shared.Tracing;
using System.Diagnostics;

public class CourseService
{
    public async Task<Course> GetCourseAsync(int id)
    {
        // Criar span customizado
        using var activity = ActivitySourceProvider.StartActivity(
            "CourseService",
            "GetCourse",
            ActivityKind.Internal
        );

        // Adicionar tags
        ActivitySourceProvider.AddTag("course.id", id);
        ActivitySourceProvider.AddTag("operation", "read");

        try
        {
            var course = await _repository.GetByIdAsync(id);

            // Adicionar evento
            ActivitySourceProvider.AddEvent("CourseFound", 
                ("course.name", course.Name),
                ("course.level", course.Level)
            );

            return course;
        }
        catch (Exception ex)
        {
            // Registrar exceção
            ActivitySourceProvider.RecordException(ex);
            throw;
        }
    }
}
```

---

## 📊 Instrumentação Automática

O OpenTelemetry já instrumenta automaticamente:

### ASP.NET Core
- ✅ Requisições HTTP (entrada)
- ✅ Roteamento
- ✅ Middleware pipeline
- ✅ Exceções

### HttpClient
- ✅ Requisições HTTP (saída)
- ✅ Timeouts
- ✅ Erros de conexão

### SQL Server
- ✅ Queries SQL
- ✅ Comandos
- ✅ Transações
- ✅ Erros de banco

---

## 🎨 Exemplos de Uso

### 1. Rastrear Operação de Negócio

```csharp
public async Task<bool> EnrollStudentAsync(int studentId, int courseId)
{
    using var activity = ActivitySourceProvider.StartActivity(
        "CourseService",
        "EnrollStudent",
        ActivityKind.Internal
    );

    ActivitySourceProvider.AddTag("student.id", studentId);
    ActivitySourceProvider.AddTag("course.id", courseId);

    // Verificar pré-requisitos
    var hasPrerequisites = await CheckPrerequisitesAsync(courseId, studentId);
    ActivitySourceProvider.AddEvent("PrerequisitesChecked", 
        ("result", hasPrerequisites)
    );

    if (!hasPrerequisites)
    {
        ActivitySourceProvider.AddTag("enrollment.status", "rejected");
        return false;
    }

    // Criar matrícula
    await _repository.CreateEnrollmentAsync(studentId, courseId);
    ActivitySourceProvider.AddEvent("EnrollmentCreated");

    // Enviar notificação
    await _notificationService.SendEnrollmentConfirmationAsync(studentId);
    ActivitySourceProvider.AddEvent("NotificationSent");

    ActivitySourceProvider.AddTag("enrollment.status", "success");
    return true;
}
```

### 2. Rastrear Chamada entre Serviços

```csharp
public async Task<CourseWithProgress> GetCourseWithProgressAsync(int courseId, int userId)
{
    using var activity = ActivitySourceProvider.StartActivity(
        "ApiGateway",
        "GetCourseWithProgress",
        ActivityKind.Server
    );

    ActivitySourceProvider.AddTag("course.id", courseId);
    ActivitySourceProvider.AddTag("user.id", userId);

    // Chamar CourseService (span filho automático via HttpClient)
    var course = await _courseClient.GetCourseAsync(courseId);
    ActivitySourceProvider.AddEvent("CourseRetrieved");

    // Chamar ProgressService (span filho automático via HttpClient)
    var progress = await _progressClient.GetProgressAsync(userId, courseId);
    ActivitySourceProvider.AddEvent("ProgressRetrieved");

    return new CourseWithProgress
    {
        Course = course,
        Progress = progress
    };
}
```

### 3. Rastrear Operação Assíncrona

```csharp
public async Task ProcessChallengeSubmissionAsync(ChallengeSubmission submission)
{
    using var activity = ActivitySourceProvider.StartActivity(
        "ChallengeService",
        "ProcessSubmission",
        ActivityKind.Consumer
    );

    ActivitySourceProvider.AddTag("submission.id", submission.Id);
    ActivitySourceProvider.AddTag("challenge.id", submission.ChallengeId);

    try
    {
        // Executar código
        var result = await _executionService.ExecuteCodeAsync(submission.Code);
        ActivitySourceProvider.AddEvent("CodeExecuted", 
            ("status", result.Status),
            ("duration_ms", result.Duration)
        );

        // Avaliar resultado
        var score = await _evaluationService.EvaluateAsync(result);
        ActivitySourceProvider.AddEvent("ResultEvaluated", 
            ("score", score)
        );

        // Atualizar progresso
        await _progressService.UpdateProgressAsync(submission.UserId, score);
        ActivitySourceProvider.AddEvent("ProgressUpdated");

        ActivitySourceProvider.AddTag("submission.status", "completed");
    }
    catch (Exception ex)
    {
        ActivitySourceProvider.RecordException(ex);
        ActivitySourceProvider.AddTag("submission.status", "failed");
        throw;
    }
}
```

---

## 🔍 Visualizando Traces no Jaeger

### 1. Acessar Jaeger UI

Abra http://localhost:16686

### 2. Buscar Traces

- **Service**: Selecione o serviço (ex: CourseService)
- **Operation**: Selecione a operação (ex: GET /api/courses/{id})
- **Tags**: Filtre por tags (ex: http.status_code=500)
- **Lookback**: Período de busca (ex: Last Hour)

### 3. Analisar Trace

Cada trace mostra:
- **Timeline**: Visualização temporal dos spans
- **Spans**: Lista hierárquica de operações
- **Duration**: Tempo de cada operação
- **Tags**: Metadados de cada span
- **Logs**: Eventos registrados

### 4. Identificar Problemas

- **Latência**: Spans com duração alta
- **Erros**: Spans com status de erro
- **Gargalos**: Operações que bloqueiam outras
- **Dependências**: Chamadas entre serviços

---

## 📊 Métricas vs Traces vs Logs

### Métricas (Prometheus)
- **O que**: Agregações numéricas (contadores, histogramas)
- **Quando**: Monitoramento contínuo, alertas
- **Exemplo**: "API teve 1000 req/s com p95 de 200ms"

### Traces (Jaeger)
- **O que**: Fluxo de execução de requisições individuais
- **Quando**: Debugging, análise de performance
- **Exemplo**: "Requisição X levou 500ms, sendo 300ms no banco"

### Logs (Serilog)
- **O que**: Eventos discretos com contexto
- **Quando**: Debugging, auditoria
- **Exemplo**: "Usuário 123 criou curso 456 às 10:30"

### Correlação

Todos os três são correlacionados via **Correlation ID**:

```
Trace ID: abc123
├─ Span 1: API Gateway (100ms)
│  └─ Log: "Request received" [correlation_id=abc123]
├─ Span 2: Course Service (80ms)
│  ├─ Log: "Fetching course" [correlation_id=abc123]
│  └─ Metric: database_query_duration_seconds{operation="GetCourse"}
└─ Span 3: Database Query (60ms)
   └─ Log: "Query executed" [correlation_id=abc123]
```

---

## 🎯 Best Practices

### 1. Nomeação de Spans

```csharp
// ❌ Ruim: genérico demais
StartActivity("Operation");

// ✅ Bom: específico e descritivo
StartActivity("CourseService", "GetCourseById");
StartActivity("CourseService", "CreateCourse");
StartActivity("CourseService", "UpdateCourseContent");
```

### 2. Tags Úteis

```csharp
// Tags de negócio
AddTag("user.id", userId);
AddTag("course.id", courseId);
AddTag("operation.type", "read");

// Tags técnicas
AddTag("cache.hit", true);
AddTag("retry.attempt", 2);
AddTag("batch.size", 100);

// Tags de resultado
AddTag("result.count", courses.Count);
AddTag("result.status", "success");
```

### 3. Eventos Significativos

```csharp
// ✅ Eventos importantes
AddEvent("ValidationStarted");
AddEvent("CacheHit", ("key", cacheKey));
AddEvent("RetryAttempt", ("attempt", 2), ("reason", "timeout"));
AddEvent("BatchProcessed", ("count", 100));

// ❌ Evitar eventos triviais
AddEvent("VariableAssigned"); // Muito granular
AddEvent("LoopIteration");    // Muito frequente
```

### 4. Exceções

```csharp
try
{
    await ProcessAsync();
}
catch (ValidationException ex)
{
    // Registrar exceção no span
    RecordException(ex);
    
    // Adicionar contexto adicional
    AddTag("validation.field", ex.FieldName);
    AddTag("validation.error", ex.Message);
    
    throw;
}
```

### 5. Spans Assíncronos

```csharp
// ✅ Correto: using garante dispose
using var activity = StartActivity("LongOperation");
await Task.Delay(1000);
// Span é fechado automaticamente

// ❌ Incorreto: span pode não ser fechado
var activity = StartActivity("LongOperation");
await Task.Delay(1000);
// Esqueceu de chamar activity.Dispose()
```

---

## 🔧 Configuração Avançada

### Sampling

Controlar quantos traces são coletados:

```csharp
builder.Services.AddDistributedTracing(
    serviceName: "CourseService",
    jaegerEndpoint: "localhost"
)
.WithTracing(tracerBuilder =>
{
    // Sempre coletar (desenvolvimento)
    tracerBuilder.SetSampler(new AlwaysOnSampler());
    
    // Coletar 10% (produção)
    // tracerBuilder.SetSampler(new TraceIdRatioBasedSampler(0.1));
});
```

### Filtros

Excluir endpoints específicos:

```csharp
.AddAspNetCoreInstrumentation(options =>
{
    options.Filter = (httpContext) =>
    {
        var path = httpContext.Request.Path.Value ?? "";
        
        // Não rastrear health checks, metrics, swagger
        return !path.Contains("/health") 
            && !path.Contains("/metrics")
            && !path.Contains("/swagger");
    };
})
```

### Enriquecimento

Adicionar informações customizadas:

```csharp
.AddAspNetCoreInstrumentation(options =>
{
    options.Enrich = (activity, eventName, rawObject) =>
    {
        if (eventName == "OnStartActivity" && rawObject is HttpRequest request)
        {
            activity.SetTag("user.agent", request.Headers["User-Agent"].ToString());
            activity.SetTag("client.ip", request.HttpContext.Connection.RemoteIpAddress?.ToString());
        }
    };
})
```

---

## 🐛 Troubleshooting

### Traces não aparecem no Jaeger

1. Verificar se Jaeger está rodando:
```bash
curl http://localhost:16686
```

2. Verificar configuração do serviço:
```bash
# Logs devem mostrar "OpenTelemetry initialized"
docker logs course-service
```

3. Verificar conectividade:
```bash
# Porta 6831 deve estar acessível
telnet localhost 6831
```

### Spans não têm parent correto

- Verificar se `Activity.Current` está sendo propagado
- Usar `ActivityKind.Client` para chamadas HTTP
- Usar `ActivityKind.Server` para endpoints

### Performance degradada

- Reduzir sampling rate em produção
- Desabilitar `SetDbStatementForText` para queries SQL
- Filtrar endpoints de alta frequência

---

## 📚 Recursos Adicionais

- [OpenTelemetry .NET Documentation](https://opentelemetry.io/docs/instrumentation/net/)
- [Jaeger Documentation](https://www.jaegertracing.io/docs/)
- [Distributed Tracing Best Practices](https://opentelemetry.io/docs/concepts/signals/traces/)

---

## 🎉 Benefícios

### Desenvolvimento
- ✅ Debug mais fácil de problemas complexos
- ✅ Visualização clara do fluxo de execução
- ✅ Identificação rápida de gargalos

### Operação
- ✅ Análise de performance em produção
- ✅ Identificação de dependências
- ✅ Root cause analysis de erros

### Negócio
- ✅ Entendimento do comportamento do usuário
- ✅ Otimização de fluxos críticos
- ✅ SLA monitoring

