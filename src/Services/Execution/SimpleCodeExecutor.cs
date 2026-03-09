using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.Logging;

namespace Execution.Service;

public class SimpleCodeExecutor
{
    private readonly ILogger<SimpleCodeExecutor> _logger;
    private readonly AspNetCoreExecutor? _aspNetCoreExecutor;

    public SimpleCodeExecutor(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<SimpleCodeExecutor>();
        
        // Create AspNetCoreExecutor with proper logger
        try
        {
            var aspNetLogger = loggerFactory.CreateLogger<AspNetCoreExecutor>();
            _aspNetCoreExecutor = new AspNetCoreExecutor(aspNetLogger);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to create AspNetCoreExecutor, ASP.NET Core execution will be disabled");
            _aspNetCoreExecutor = null;
        }
    }

    public async Task<SimpleExecutionResult> ExecuteAsync(string code)
    {
        // Check if this is ASP.NET Core code and executor is available
        if (_aspNetCoreExecutor != null && _aspNetCoreExecutor.IsAspNetCoreCode(code))
        {
            _logger.LogInformation("Detected ASP.NET Core code, using specialized executor");
            return await _aspNetCoreExecutor.ExecuteAsync(code);
        }

        // Otherwise, use standard console execution
        return await ExecuteConsoleCodeAsync(code);
    }

    private async Task<SimpleExecutionResult> ExecuteConsoleCodeAsync(string code)
    {
        var stopwatch = Stopwatch.StartNew();
        var result = new SimpleExecutionResult();

        try
        {
            // Compile
            var syntaxTree = CSharpSyntaxTree.ParseText(code);
            
            // Build list of references
            var references = new List<MetadataReference>
            {
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Console).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(System.Runtime.AssemblyTargetedPatchBandAttribute).Assembly.Location),
                MetadataReference.CreateFromFile(Assembly.Load("System.Runtime").Location),
                MetadataReference.CreateFromFile(Assembly.Load("System.Console").Location),
            };

            // Try to add ASP.NET Core references if available (for web examples)
            try
            {
                references.Add(MetadataReference.CreateFromFile(Assembly.Load("Microsoft.AspNetCore.Mvc.Core").Location));
                references.Add(MetadataReference.CreateFromFile(Assembly.Load("Microsoft.AspNetCore.Mvc.Abstractions").Location));
                references.Add(MetadataReference.CreateFromFile(Assembly.Load("Microsoft.AspNetCore.Http.Abstractions").Location));
                references.Add(MetadataReference.CreateFromFile(Assembly.Load("System.Collections").Location));
                references.Add(MetadataReference.CreateFromFile(Assembly.Load("System.Linq").Location));
            }
            catch
            {
                // ASP.NET Core references not available, continue with basic references
                _logger.LogDebug("ASP.NET Core references not available for code execution");
            }

            var compilation = CSharpCompilation.Create(
                "DynamicCode",
                new[] { syntaxTree },
                references,
                new CSharpCompilationOptions(OutputKind.ConsoleApplication));

            using var ms = new MemoryStream();
            var emitResult = compilation.Emit(ms);

            if (!emitResult.Success)
            {
                var errors = emitResult.Diagnostics
                    .Where(d => d.Severity == DiagnosticSeverity.Error)
                    .Select(d => $"Line {d.Location.GetLineSpan().StartLinePosition.Line + 1}: {d.GetMessage()}");
                
                result.Status = "Failed";
                result.Error = string.Join("\n", errors);
                result.ExecutionTimeMs = (int)stopwatch.ElapsedMilliseconds;
                return result;
            }

            // Execute
            ms.Seek(0, SeekOrigin.Begin);
            var assembly = Assembly.Load(ms.ToArray());
            var entryPoint = assembly.EntryPoint;

            if (entryPoint == null)
            {
                result.Status = "Failed";
                result.Error = "Programa não contém um método Main() estático.\n\n" +
                               "💡 Para código console, use:\n\n" +
                               "using System;\n\n" +
                               "class Program\n" +
                               "{\n" +
                               "    static void Main()\n" +
                               "    {\n" +
                               "        Console.WriteLine(\"Hello World!\");\n" +
                               "    }\n" +
                               "}";
                return result;
            }

            var outputBuilder = new StringBuilder();
            var originalOut = Console.Out;

            try
            {
                using var writer = new StringWriter(outputBuilder);
                Console.SetOut(writer);

                var parameters = entryPoint.GetParameters().Length == 0 
                    ? null 
                    : new object[] { Array.Empty<string>() };

                await Task.Run(() => entryPoint.Invoke(null, parameters));
                
                result.Output = outputBuilder.ToString();
                result.Status = "Completed";
            }
            finally
            {
                Console.SetOut(originalOut);
            }

            result.ExecutionTimeMs = (int)stopwatch.ElapsedMilliseconds;
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Execution failed");
            result.Status = "Failed";
            result.Error = ex.InnerException?.Message ?? ex.Message;
            result.ExecutionTimeMs = (int)stopwatch.ElapsedMilliseconds;
            return result;
        }
    }
}

public class SimpleExecutionResult
{
    public string Status { get; set; } = "Completed";
    public string Output { get; set; } = "";
    public string? Error { get; set; }
    public int ExecutionTimeMs { get; set; }
}
