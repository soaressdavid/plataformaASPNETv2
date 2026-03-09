using Docker.DotNet;
using Docker.DotNet.Models;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace Execution.Service.Services;

/// <summary>
/// Executes code inside Docker containers with resource limits and isolation
/// Validates: Requirements 7.5, 7.6, 7.9, 21.2, 21.3, 21.4
/// Task 6.4: Implement code execution in Docker containers
/// </summary>
public class DockerCodeExecutor : IDockerCodeExecutor
{
    private readonly IDockerClient _dockerClient;
    private readonly IContainerPoolManager _containerPoolManager;
    private readonly ProhibitedCodeScanner _codeScanner;
    private readonly ILogger<DockerCodeExecutor> _logger;

    // Execution limits
    private const int ExecutionTimeoutSeconds = 60;
    private const long MemoryLimitBytes = 512 * 1024 * 1024; // 512MB
    private const long CpuQuota = 100000; // 1 CPU core

    public DockerCodeExecutor(
        IDockerClient dockerClient,
        IContainerPoolManager containerPoolManager,
        ProhibitedCodeScanner codeScanner,
        ILogger<DockerCodeExecutor> logger)
    {
        _dockerClient = dockerClient;
        _containerPoolManager = containerPoolManager;
        _codeScanner = codeScanner;
        _logger = logger;
    }

    /// <summary>
    /// Executes code in an isolated Docker container with resource limits
    /// </summary>
    public async Task<DockerExecutionResult> ExecuteAsync(
        string code,
        string? sessionId = null,
        int timeoutSeconds = ExecutionTimeoutSeconds,
        CancellationToken cancellationToken = default)
    {
        var stopwatch = Stopwatch.StartNew();
        var result = new DockerExecutionResult
        {
            JobId = Guid.NewGuid(),
            Status = ExecutionStatus.Running
        };

        string? containerId = null;

        try
        {
            // Step 1: Scan for malicious code
            _logger.LogInformation("Scanning code for prohibited patterns");
            var scanResult = _codeScanner.ScanCode(code);
            
            if (!scanResult.IsSafe)
            {
                _logger.LogWarning("Code rejected: {Violations}", string.Join(", ", scanResult.Violations.Select(v => v.Reason)));
                result.Status = ExecutionStatus.Rejected;
                result.Error = $"Code rejected: {string.Join(", ", scanResult.Violations.Select(v => $"Line {v.Line}: {v.Reason}"))}";
                result.ExecutionTimeMs = stopwatch.ElapsedMilliseconds;
                return result;
            }

            // Step 2: Compile code with Roslyn
            _logger.LogInformation("Compiling code with Roslyn");
            var compilationResult = await CompileCodeAsync(code, cancellationToken);
            
            if (!compilationResult.Success)
            {
                _logger.LogWarning("Compilation failed: {Errors}", string.Join("; ", compilationResult.Errors));
                result.Status = ExecutionStatus.CompilationError;
                result.Error = string.Join("\n", compilationResult.Errors);
                result.CompilationErrors = compilationResult.Errors;
                result.ExecutionTimeMs = stopwatch.ElapsedMilliseconds;
                return result;
            }

            // Step 3: Acquire container from pool
            _logger.LogInformation("Acquiring container from pool (session: {SessionId})", sessionId ?? "none");
            containerId = await _containerPoolManager.AcquireContainerAsync(sessionId, cancellationToken);
            
            if (containerId == null)
            {
                _logger.LogError("Failed to acquire container from pool");
                result.Status = ExecutionStatus.Failed;
                result.Error = "Container pool exhausted. Please try again later.";
                result.ExecutionTimeMs = stopwatch.ElapsedMilliseconds;
                return result;
            }

            _logger.LogInformation("Acquired container {ContainerId}", containerId);

            // Step 4: Copy compiled assembly to container
            await CopyAssemblyToContainerAsync(containerId, compilationResult.AssemblyBytes!, cancellationToken);

            // Step 5: Execute code in container with timeout
            _logger.LogInformation("Executing code in container with {Timeout}s timeout", timeoutSeconds);
            var executionResult = await ExecuteInContainerAsync(
                containerId,
                timeoutSeconds,
                cancellationToken);

            // Step 6: Populate result
            result.Status = executionResult.TimedOut ? ExecutionStatus.Timeout :
                           executionResult.MemoryExceeded ? ExecutionStatus.MemoryExceeded :
                           executionResult.ExitCode == 0 ? ExecutionStatus.Completed :
                           ExecutionStatus.RuntimeError;
            
            result.Output = executionResult.Output;
            result.Error = executionResult.Error;
            result.ExitCode = executionResult.ExitCode;
            result.TimedOut = executionResult.TimedOut;
            result.MemoryExceeded = executionResult.MemoryExceeded;
            result.ExecutionTimeMs = stopwatch.ElapsedMilliseconds;

            _logger.LogInformation("Execution completed: Status={Status}, ExitCode={ExitCode}, Time={Time}ms",
                result.Status, result.ExitCode, result.ExecutionTimeMs);

            return result;
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("Execution cancelled");
            result.Status = ExecutionStatus.Cancelled;
            result.Error = "Execution was cancelled";
            result.ExecutionTimeMs = stopwatch.ElapsedMilliseconds;
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Execution failed with exception");
            result.Status = ExecutionStatus.Failed;
            result.Error = $"Internal error: {ex.Message}";
            result.ExecutionTimeMs = stopwatch.ElapsedMilliseconds;
            return result;
        }
        finally
        {
            // Step 7: Release container back to pool
            if (containerId != null)
            {
                _logger.LogInformation("Releasing container {ContainerId} back to pool", containerId);
                await _containerPoolManager.ReleaseContainerAsync(containerId, sessionId, cancellationToken);
            }
        }
    }

    /// <summary>
    /// Compiles C# code using Roslyn
    /// </summary>
    private async Task<CompilationResult> CompileCodeAsync(string code, CancellationToken cancellationToken)
    {
        return await Task.Run(() =>
        {
            try
            {
                var syntaxTree = CSharpSyntaxTree.ParseText(code);

                // Build references
                var references = new List<MetadataReference>
                {
                    MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                    MetadataReference.CreateFromFile(typeof(Console).Assembly.Location),
                    MetadataReference.CreateFromFile(typeof(System.Runtime.AssemblyTargetedPatchBandAttribute).Assembly.Location),
                    MetadataReference.CreateFromFile(Assembly.Load("System.Runtime").Location),
                    MetadataReference.CreateFromFile(Assembly.Load("System.Console").Location),
                    MetadataReference.CreateFromFile(Assembly.Load("System.Collections").Location),
                    MetadataReference.CreateFromFile(Assembly.Load("System.Linq").Location),
                };

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
                        .Select(d => $"Line {d.Location.GetLineSpan().StartLinePosition.Line + 1}: {d.GetMessage()}")
                        .ToList();

                    return new CompilationResult
                    {
                        Success = false,
                        Errors = errors
                    };
                }

                return new CompilationResult
                {
                    Success = true,
                    AssemblyBytes = ms.ToArray()
                };
            }
            catch (Exception ex)
            {
                return new CompilationResult
                {
                    Success = false,
                    Errors = new List<string> { $"Compilation exception: {ex.Message}" }
                };
            }
        }, cancellationToken);
    }

    /// <summary>
    /// Copies compiled assembly to container workspace
    /// </summary>
    private async Task CopyAssemblyToContainerAsync(
        string containerId,
        byte[] assemblyBytes,
        CancellationToken cancellationToken)
    {
        try
        {
            // Create tar archive with assembly
            using var tarStream = new MemoryStream();
            using (var tarWriter = new System.Formats.Tar.TarWriter(tarStream, leaveOpen: true))
            {
                var entry = new System.Formats.Tar.PaxTarEntry(System.Formats.Tar.TarEntryType.RegularFile, "Program.dll")
                {
                    DataStream = new MemoryStream(assemblyBytes),
                    Mode = (System.IO.UnixFileMode)Convert.ToInt32("644", 8) // rw-r--r--
                };
                
                await tarWriter.WriteEntryAsync(entry, cancellationToken);
            }

            tarStream.Seek(0, SeekOrigin.Begin);

            // Copy to container
            await _dockerClient.Containers.ExtractArchiveToContainerAsync(
                containerId,
                new ContainerPathStatParameters { Path = "/workspace", AllowOverwriteDirWithFile = true },
                tarStream,
                cancellationToken);

            _logger.LogDebug("Copied assembly to container {ContainerId}", containerId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to copy assembly to container {ContainerId}", containerId);
            throw;
        }
    }

    /// <summary>
    /// Executes the compiled assembly in the container
    /// Validates: Requirements 7.5, 7.6, 7.9 (execute with resource limits, capture output, 60s timeout)
    /// </summary>
    private async Task<ContainerExecutionResult> ExecuteInContainerAsync(
        string containerId,
        int timeoutSeconds,
        CancellationToken cancellationToken)
    {
        var result = new ContainerExecutionResult();
        var stopwatch = Stopwatch.StartNew();

        try
        {
            // Create exec instance
            var execParams = new ContainerExecCreateParameters
            {
                Cmd = new[] { "dotnet", "/workspace/Program.dll" },
                AttachStdout = true,
                AttachStderr = true,
                WorkingDir = "/workspace"
            };

            var execResponse = await _dockerClient.Exec.ExecCreateContainerAsync(
                containerId,
                execParams,
                cancellationToken);

            // Start execution with timeout
            using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            cts.CancelAfter(TimeSpan.FromSeconds(timeoutSeconds));

            var outputBuilder = new StringBuilder();
            var errorBuilder = new StringBuilder();

            try
            {
                var stream = await _dockerClient.Exec.StartAndAttachContainerExecAsync(
                    execResponse.ID,
                    false,
                    cts.Token);

                // Read output
                await stream.CopyOutputToAsync(
                    Stream.Null,
                    outputBuilder,
                    errorBuilder,
                    cts.Token);

                result.Output = outputBuilder.ToString();
                result.Error = errorBuilder.ToString();
            }
            catch (OperationCanceledException) when (cts.Token.IsCancellationRequested && !cancellationToken.IsCancellationRequested)
            {
                // Timeout occurred
                result.TimedOut = true;
                result.Error = $"Execution timed out after {timeoutSeconds} seconds";
                _logger.LogWarning("Container {ContainerId} execution timed out", containerId);
            }

            // Inspect exec to get exit code
            var execInspect = await _dockerClient.Exec.InspectContainerExecAsync(execResponse.ID, cancellationToken);
            result.ExitCode = (int)execInspect.ExitCode;

            // Check for OOM (Out of Memory)
            var containerInspect = await _dockerClient.Containers.InspectContainerAsync(containerId, cancellationToken);
            if (containerInspect.State.OOMKilled)
            {
                result.MemoryExceeded = true;
                result.Error = "Memory limit exceeded (512MB)";
                _logger.LogWarning("Container {ContainerId} killed due to OOM", containerId);
            }

            result.ExecutionTimeMs = stopwatch.ElapsedMilliseconds;
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to execute in container {ContainerId}", containerId);
            result.Error = $"Execution error: {ex.Message}";
            result.ExitCode = -1;
            result.ExecutionTimeMs = stopwatch.ElapsedMilliseconds;
            return result;
        }
    }
}

/// <summary>
/// Result of Docker code execution
/// </summary>
public class DockerExecutionResult
{
    public Guid JobId { get; set; }
    public ExecutionStatus Status { get; set; }
    public string? Output { get; set; }
    public string? Error { get; set; }
    public int? ExitCode { get; set; }
    public long ExecutionTimeMs { get; set; }
    public bool TimedOut { get; set; }
    public bool MemoryExceeded { get; set; }
    public List<string>? CompilationErrors { get; set; }
}

/// <summary>
/// Execution status enum
/// </summary>
public enum ExecutionStatus
{
    Queued,
    Running,
    Completed,
    CompilationError,
    RuntimeError,
    Timeout,
    MemoryExceeded,
    Rejected,
    Failed,
    Cancelled
}

/// <summary>
/// Internal compilation result
/// </summary>
internal class CompilationResult
{
    public bool Success { get; set; }
    public byte[]? AssemblyBytes { get; set; }
    public List<string> Errors { get; set; } = new();
}

/// <summary>
/// Internal container execution result
/// </summary>
internal class ContainerExecutionResult
{
    public string Output { get; set; } = "";
    public string Error { get; set; } = "";
    public int ExitCode { get; set; }
    public long ExecutionTimeMs { get; set; }
    public bool TimedOut { get; set; }
    public bool MemoryExceeded { get; set; }
}

/// <summary>
/// Interface for Docker code executor
/// </summary>
public interface IDockerCodeExecutor
{
    Task<DockerExecutionResult> ExecuteAsync(
        string code,
        string? sessionId = null,
        int timeoutSeconds = 60,
        CancellationToken cancellationToken = default);
}

// Extension method for stream output copying
internal static class StreamExtensions
{
    public static async Task CopyOutputToAsync(
        this MultiplexedStream stream,
        Stream stdout,
        StringBuilder stdoutBuilder,
        StringBuilder stderrBuilder,
        CancellationToken cancellationToken)
    {
        var buffer = new byte[4096];
        
        while (true)
        {
            var readResult = await stream.ReadOutputAsync(buffer, 0, buffer.Length, cancellationToken);
            
            if (readResult.EOF)
                break;

            var text = Encoding.UTF8.GetString(buffer, 0, readResult.Count);
            
            if (readResult.Target == MultiplexedStream.TargetStream.StandardOut)
            {
                stdoutBuilder.Append(text);
            }
            else if (readResult.Target == MultiplexedStream.TargetStream.StandardError)
            {
                stderrBuilder.Append(text);
            }
        }
    }
}
