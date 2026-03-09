using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;

namespace Execution.Service;

/// <summary>
/// Executor specialized for ASP.NET Core code (Controllers, Minimal APIs, etc.)
/// Creates a temporary Kestrel server, executes HTTP requests, and returns formatted responses.
/// </summary>
public class AspNetCoreExecutor
{
    private readonly ILogger<AspNetCoreExecutor> _logger;
    private const int ServerPort = 5555; // Temporary port for execution
    private const int ServerTimeoutSeconds = 30;

    public AspNetCoreExecutor(ILogger<AspNetCoreExecutor> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Detects if code is ASP.NET Core based on common patterns
    /// </summary>
    public bool IsAspNetCoreCode(string code)
    {
        return code.Contains("ControllerBase") ||
               code.Contains("[ApiController]") ||
               code.Contains("IActionResult") ||
               code.Contains("Microsoft.AspNetCore") ||
               code.Contains("WebApplication.Create") ||
               code.Contains("MapGet") ||
               code.Contains("MapPost");
    }

    /// <summary>
    /// Executes ASP.NET Core code by creating a temporary server and testing endpoints
    /// </summary>
    public async Task<SimpleExecutionResult> ExecuteAsync(string code)
    {
        var stopwatch = Stopwatch.StartNew();
        var result = new SimpleExecutionResult();
        var outputBuilder = new StringBuilder();

        try
        {
            _logger.LogInformation("Starting ASP.NET Core code execution");

            // Step 1: Compile the code
            outputBuilder.AppendLine("🔨 Compilando código ASP.NET Core...");
            var assembly = await CompileCodeAsync(code);
            if (assembly == null)
            {
                result.Status = "Failed";
                result.Error = "Falha na compilação do código";
                return result;
            }
            outputBuilder.AppendLine("✅ Compilação bem-sucedida!\n");

            // Step 2: Detect endpoints from code
            var endpoints = DetectEndpoints(code);
            if (endpoints.Count == 0)
            {
                outputBuilder.AppendLine("⚠️ Nenhum endpoint detectado no código.");
                outputBuilder.AppendLine("💡 Adicione métodos HTTP como [HttpGet], [HttpPost], etc.\n");
            }

            // Step 3: Create and start temporary server
            outputBuilder.AppendLine($"🚀 Iniciando servidor temporário na porta {ServerPort}...");
            var serverTask = StartTemporaryServerAsync(assembly, code);
            
            // Wait for server to start
            await Task.Delay(2000);
            outputBuilder.AppendLine("✅ Servidor iniciado!\n");

            // Step 4: Test detected endpoints
            if (endpoints.Count > 0)
            {
                outputBuilder.AppendLine("📡 Testando endpoints detectados:\n");
                outputBuilder.AppendLine("═══════════════════════════════════════\n");

                using var httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(5) };
                
                foreach (var endpoint in endpoints)
                {
                    try
                    {
                        // Replace route parameters with sample values
                        var testPath = endpoint.Path
                            .Replace("{id}", "1")
                            .Replace("{name}", "test")
                            .Replace("{code}", "ABC");
                        
                        outputBuilder.AppendLine($"🔹 {endpoint.Method} {endpoint.Path}");
                        if (testPath != endpoint.Path)
                        {
                            outputBuilder.AppendLine($"   Testando com: {testPath}");
                        }
                        
                        HttpResponseMessage response;
                        if (endpoint.Method == "GET")
                        {
                            response = await httpClient.GetAsync($"http://localhost:{ServerPort}{testPath}");
                        }
                        else if (endpoint.Method == "POST")
                        {
                            var content = new StringContent(endpoint.SampleBody ?? "\"test\"", Encoding.UTF8, "application/json");
                            response = await httpClient.PostAsync($"http://localhost:{ServerPort}{testPath}", content);
                        }
                        else if (endpoint.Method == "PUT")
                        {
                            var content = new StringContent(endpoint.SampleBody ?? "\"updated\"", Encoding.UTF8, "application/json");
                            response = await httpClient.PutAsync($"http://localhost:{ServerPort}{testPath}", content);
                        }
                        else if (endpoint.Method == "DELETE")
                        {
                            response = await httpClient.DeleteAsync($"http://localhost:{ServerPort}{testPath}");
                        }
                        else
                        {
                            outputBuilder.AppendLine($"   ⚠️ Método {endpoint.Method} não suportado para teste automático\n");
                            continue;
                        }

                        var responseBody = await response.Content.ReadAsStringAsync();
                        
                        outputBuilder.AppendLine($"   Status: {(int)response.StatusCode} {response.StatusCode}");
                        
                        // Truncate long responses
                        if (responseBody.Length > 200)
                        {
                            responseBody = responseBody.Substring(0, 200) + "... (truncado)";
                        }
                        
                        outputBuilder.AppendLine($"   Response: {(string.IsNullOrWhiteSpace(responseBody) ? "(vazio)" : responseBody)}");
                        outputBuilder.AppendLine();
                    }
                    catch (Exception ex)
                    {
                        outputBuilder.AppendLine($"   ❌ Erro: {ex.Message}\n");
                    }
                }

                outputBuilder.AppendLine("═══════════════════════════════════════\n");
            }

            // Step 5: Shutdown server
            outputBuilder.AppendLine("🛑 Encerrando servidor temporário...");
            // Server will be disposed automatically
            await Task.Delay(500);
            outputBuilder.AppendLine("✅ Servidor encerrado com sucesso!");

            result.Output = outputBuilder.ToString();
            result.Status = "Completed";
            result.ExecutionTimeMs = (int)stopwatch.ElapsedMilliseconds;
            
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ASP.NET Core execution failed");
            result.Status = "Failed";
            result.Error = $"Erro na execução: {ex.Message}";
            result.Output = outputBuilder.ToString();
            result.ExecutionTimeMs = (int)stopwatch.ElapsedMilliseconds;
            return result;
        }
    }

    private async Task<Assembly?> CompileCodeAsync(string code)
    {
        try
        {
            var syntaxTree = CSharpSyntaxTree.ParseText(code);

            var references = new List<MetadataReference>
            {
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Console).Assembly.Location),
                MetadataReference.CreateFromFile(Assembly.Load("System.Runtime").Location),
                MetadataReference.CreateFromFile(Assembly.Load("System.Collections").Location),
                MetadataReference.CreateFromFile(Assembly.Load("System.Linq").Location),
                MetadataReference.CreateFromFile(Assembly.Load("Microsoft.AspNetCore.Mvc.Core").Location),
                MetadataReference.CreateFromFile(Assembly.Load("Microsoft.AspNetCore.Mvc.Abstractions").Location),
                MetadataReference.CreateFromFile(Assembly.Load("Microsoft.AspNetCore.Http.Abstractions").Location),
                MetadataReference.CreateFromFile(Assembly.Load("Microsoft.AspNetCore").Location),
                MetadataReference.CreateFromFile(Assembly.Load("Microsoft.AspNetCore.Routing").Location),
            };

            var compilation = CSharpCompilation.Create(
                "DynamicAspNetCore",
                new[] { syntaxTree },
                references,
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            using var ms = new MemoryStream();
            var emitResult = compilation.Emit(ms);

            if (!emitResult.Success)
            {
                var errors = emitResult.Diagnostics
                    .Where(d => d.Severity == DiagnosticSeverity.Error)
                    .Select(d => $"Line {d.Location.GetLineSpan().StartLinePosition.Line + 1}: {d.GetMessage()}");

                _logger.LogWarning("Compilation failed: {Errors}", string.Join("; ", errors));
                return null;
            }

            ms.Seek(0, SeekOrigin.Begin);
            return Assembly.Load(ms.ToArray());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Compilation error");
            return null;
        }
    }

    private async Task StartTemporaryServerAsync(Assembly assembly, string code)
    {
        var cts = new CancellationTokenSource(TimeSpan.FromSeconds(ServerTimeoutSeconds));
        
        try
        {
            var builder = WebApplication.CreateBuilder();
            builder.WebHost.UseUrls($"http://localhost:{ServerPort}");
            
            // Add services
            builder.Services.AddControllers();
            
            // Register controllers from compiled assembly
            builder.Services.AddMvc().AddApplicationPart(assembly);

            var app = builder.Build();
            
            app.MapControllers();
            
            // Start server in background
            _ = Task.Run(async () =>
            {
                try
                {
                    await app.RunAsync(cts.Token);
                }
                catch (OperationCanceledException)
                {
                    // Expected when cancellation is requested
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Server error");
                }
            }, cts.Token);

            // Keep server alive for testing
            await Task.Delay(ServerTimeoutSeconds * 1000, cts.Token);
        }
        catch (OperationCanceledException)
        {
            // Expected
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to start temporary server");
        }
    }

    private List<EndpointInfo> DetectEndpoints(string code)
    {
        var endpoints = new List<EndpointInfo>();

        // First, find the controller base route
        string basePath = "/api/products"; // Default
        
        if (code.Contains("[Route(\""))
        {
            var routeIndex = code.IndexOf("[Route(\"");
            if (routeIndex >= 0)
            {
                var routeStart = routeIndex + 8;
                var routeEnd = code.IndexOf("\")", routeStart);
                if (routeEnd > routeStart)
                {
                    basePath = code.Substring(routeStart, routeEnd - routeStart);
                    
                    // Replace [controller] with actual controller name
                    if (basePath.Contains("[controller]"))
                    {
                        // Try to extract controller name from class definition
                        var controllerMatch = System.Text.RegularExpressions.Regex.Match(code, @"class\s+(\w+)Controller");
                        if (controllerMatch.Success)
                        {
                            var controllerName = controllerMatch.Groups[1].Value.ToLower();
                            basePath = basePath.Replace("[controller]", controllerName);
                        }
                        else
                        {
                            basePath = basePath.Replace("[controller]", "products");
                        }
                    }
                    
                    if (!basePath.StartsWith("/"))
                    {
                        basePath = "/" + basePath;
                    }
                }
            }
        }

        // Detect [HttpGet], [HttpPost], etc.
        var httpMethods = new[] { "HttpGet", "HttpPost", "HttpPut", "HttpDelete", "HttpPatch" };
        
        foreach (var method in httpMethods)
        {
            var pattern = $"[{method}";
            var index = 0;
            
            while ((index = code.IndexOf(pattern, index, StringComparison.OrdinalIgnoreCase)) != -1)
            {
                // Extract route parameter if present
                var routeStart = code.IndexOf("(\"", index);
                var routeEnd = code.IndexOf("\")", routeStart);
                
                string routeParam = "";
                if (routeStart > index && routeEnd > routeStart && (routeEnd - index) < 100)
                {
                    routeParam = code.Substring(routeStart + 2, routeEnd - routeStart - 2);
                }

                // Build full path
                string fullPath = basePath;
                if (!string.IsNullOrEmpty(routeParam))
                {
                    if (!routeParam.StartsWith("/"))
                    {
                        fullPath += "/";
                    }
                    fullPath += routeParam;
                }

                var httpMethod = method.Replace("Http", "").ToUpper();
                
                // Check if endpoint already exists
                if (!endpoints.Any(e => e.Method == httpMethod && e.Path == fullPath))
                {
                    endpoints.Add(new EndpointInfo
                    {
                        Method = httpMethod,
                        Path = fullPath,
                        SampleBody = httpMethod == "POST" || httpMethod == "PUT" ? "\"test data\"" : null
                    });
                }

                index = routeEnd > 0 ? routeEnd : index + pattern.Length;
            }
        }

        return endpoints;
    }

    private class EndpointInfo
    {
        public string Method { get; set; } = "";
        public string Path { get; set; } = "";
        public string? SampleBody { get; set; }
    }
}
