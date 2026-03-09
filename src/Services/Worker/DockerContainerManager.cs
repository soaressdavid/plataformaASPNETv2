using Docker.DotNet;
using Docker.DotNet.Models;
using System.Diagnostics;

namespace Worker.Service;

/// <summary>
/// Manages Docker containers for code execution
/// </summary>
public class DockerContainerManager : IDockerContainerManager
{
    private readonly DockerClient _dockerClient;

    public DockerContainerManager(DockerClient dockerClient)
    {
        _dockerClient = dockerClient ?? throw new ArgumentNullException(nameof(dockerClient));
    }

    public async Task<string> CreateContainerAsync(string code, List<string> files, string entryPoint)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("Code cannot be null or empty", nameof(code));

        var createParams = new CreateContainerParameters
        {
            Image = "mcr.microsoft.com/dotnet/sdk:8.0-alpine",
            HostConfig = new HostConfig
            {
                Memory = 512 * 1024 * 1024, // 512MB
                CPUQuota = 100000, // 1 CPU
                PidsLimit = 50,
                NetworkMode = "none"
            }
        };

        var response = await _dockerClient.Containers.CreateContainerAsync(createParams);
        return response.ID;
    }

    public async Task<ContainerExecutionResult> StartContainerAsync(string containerId, TimeSpan timeout)
    {
        if (string.IsNullOrWhiteSpace(containerId))
            throw new ArgumentException("Container ID cannot be null or empty", nameof(containerId));

        var stopwatch = Stopwatch.StartNew();
        var result = new ContainerExecutionResult();

        try
        {
            await _dockerClient.Containers.StartContainerAsync(containerId, new ContainerStartParameters());

            // Wait for container to finish or timeout
            var waitTask = _dockerClient.Containers.WaitContainerAsync(containerId);
            var completedTask = await Task.WhenAny(waitTask, Task.Delay(timeout));

            if (completedTask == waitTask)
            {
                var waitResponse = await waitTask;
                result.ExitCode = (int)waitResponse.StatusCode;
            }
            else
            {
                result.TimedOut = true;
                result.ExitCode = -1;
            }

            // Get logs
            var logsParams = new ContainerLogsParameters
            {
                ShowStdout = true,
                ShowStderr = true
            };

            using var logs = await _dockerClient.Containers.GetContainerLogsAsync(containerId, false, logsParams, CancellationToken.None);
            var (stdout, stderr) = await logs.ReadOutputToEndAsync(CancellationToken.None);
            
            result.Output = stdout + stderr;
        }
        catch (Exception ex)
        {
            result.Error = ex.Message;
            result.ExitCode = -1;
        }
        finally
        {
            stopwatch.Stop();
            result.ExecutionTimeMs = stopwatch.ElapsedMilliseconds;
        }

        return result;
    }

    public async Task StopAndRemoveContainerAsync(string containerId)
    {
        if (string.IsNullOrWhiteSpace(containerId))
            throw new ArgumentException("Container ID cannot be null or empty", nameof(containerId));

        try
        {
            await _dockerClient.Containers.StopContainerAsync(containerId, new ContainerStopParameters());
            await _dockerClient.Containers.RemoveContainerAsync(containerId, new ContainerRemoveParameters());
        }
        catch
        {
            // Ignore errors during cleanup
        }
    }
}
