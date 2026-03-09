using Docker.DotNet;
using Execution.Service.Services;
using Worker.Service;

namespace Execution.Tests;

/// <summary>
/// Unit tests for DockerContainerManager
/// Tests container creation, execution, and cleanup
/// Validates Requirements 3.3, 3.4, 3.5, 3.7, 3.8, 14.1, 14.2, 14.3, 14.6
/// </summary>
public class DockerContainerManagerTests
{
    [Fact]
    public async Task CreateContainerAsync_WithEmptyCode_ThrowsArgumentException()
    {
        // Arrange
        var dockerClient = new DockerClientConfiguration().CreateClient();
        var containerManager = new DockerContainerManager(dockerClient);
        var code = "";
        var files = new List<string> { "Program.cs" };
        var entryPoint = "Program.cs";

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(
            async () => await containerManager.CreateContainerAsync(code, files, entryPoint));
    }

    [Fact]
    public async Task CreateContainerAsync_WithNullCode_ThrowsArgumentException()
    {
        // Arrange
        var dockerClient = new DockerClientConfiguration().CreateClient();
        var containerManager = new DockerContainerManager(dockerClient);
        string code = null!;
        var files = new List<string> { "Program.cs" };
        var entryPoint = "Program.cs";

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(
            async () => await containerManager.CreateContainerAsync(code, files, entryPoint));
    }

    [Fact]
    public async Task StartContainerAsync_WithEmptyContainerId_ThrowsArgumentException()
    {
        // Arrange
        var dockerClient = new DockerClientConfiguration().CreateClient();
        var containerManager = new DockerContainerManager(dockerClient);
        var containerId = "";
        var timeout = TimeSpan.FromSeconds(30);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(
            async () => await containerManager.StartContainerAsync(containerId, timeout));
    }

    [Fact]
    public async Task StartContainerAsync_WithNullContainerId_ThrowsArgumentException()
    {
        // Arrange
        var dockerClient = new DockerClientConfiguration().CreateClient();
        var containerManager = new DockerContainerManager(dockerClient);
        string containerId = null!;
        var timeout = TimeSpan.FromSeconds(30);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(
            async () => await containerManager.StartContainerAsync(containerId, timeout));
    }

    [Fact]
    public async Task StopAndRemoveContainerAsync_WithEmptyContainerId_ThrowsArgumentException()
    {
        // Arrange
        var dockerClient = new DockerClientConfiguration().CreateClient();
        var containerManager = new DockerContainerManager(dockerClient);
        var containerId = "";

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(
            async () => await containerManager.StopAndRemoveContainerAsync(containerId));
    }

    [Fact]
    public async Task StopAndRemoveContainerAsync_WithNullContainerId_ThrowsArgumentException()
    {
        // Arrange
        var dockerClient = new DockerClientConfiguration().CreateClient();
        var containerManager = new DockerContainerManager(dockerClient);
        string containerId = null!;

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(
            async () => await containerManager.StopAndRemoveContainerAsync(containerId));
    }

    [Fact]
    public void DockerContainerManager_Constructor_WithNullClient_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new DockerContainerManager(null!));
    }

    [Fact]
    public void ContainerExecutionResult_DefaultValues_AreCorrect()
    {
        // Arrange & Act
        var result = new ContainerExecutionResult();

        // Assert
        Assert.Equal(string.Empty, result.Output);
        Assert.Equal(string.Empty, result.Error);
        Assert.Equal(0, result.ExitCode);
        Assert.Equal(0, result.ExecutionTimeMs);
        Assert.False(result.TimedOut);
        Assert.False(result.MemoryExceeded);
    }

    [Fact]
    public void ContainerExecutionResult_CanSetProperties()
    {
        // Arrange & Act
        var result = new ContainerExecutionResult
        {
            Output = "Hello, World!",
            Error = "Some error",
            ExitCode = 1,
            ExecutionTimeMs = 1500,
            TimedOut = true,
            MemoryExceeded = false
        };

        // Assert
        Assert.Equal("Hello, World!", result.Output);
        Assert.Equal("Some error", result.Error);
        Assert.Equal(1, result.ExitCode);
        Assert.Equal(1500, result.ExecutionTimeMs);
        Assert.True(result.TimedOut);
        Assert.False(result.MemoryExceeded);
    }
}
