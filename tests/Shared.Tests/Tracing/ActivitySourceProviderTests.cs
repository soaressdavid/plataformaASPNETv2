using System.Diagnostics;
using FluentAssertions;
using Shared.Tracing;
using Xunit;

namespace Shared.Tests.Tracing;

public class ActivitySourceProviderTests
{
    [Fact]
    public void GetSource_ShouldReturnSameInstanceForSameName()
    {
        // Arrange
        var serviceName = "TestService";

        // Act
        var source1 = ActivitySourceProvider.GetSource(serviceName);
        var source2 = ActivitySourceProvider.GetSource(serviceName);

        // Assert
        source1.Should().BeSameAs(source2);
        source1.Name.Should().Be(serviceName);
    }

    [Fact]
    public void GetSource_ShouldReturnDifferentInstancesForDifferentNames()
    {
        // Arrange
        var serviceName1 = "TestService1";
        var serviceName2 = "TestService2";

        // Act
        var source1 = ActivitySourceProvider.GetSource(serviceName1);
        var source2 = ActivitySourceProvider.GetSource(serviceName2);

        // Assert
        source1.Should().NotBeSameAs(source2);
        source1.Name.Should().Be(serviceName1);
        source2.Name.Should().Be(serviceName2);
    }

    [Fact]
    public void StartActivity_ShouldCreateActivity()
    {
        // Arrange
        var serviceName = "TestService";
        var operationName = "TestOperation";

        // Configurar listener para capturar activities
        using var listener = new ActivityListener
        {
            ShouldListenTo = source => source.Name == serviceName,
            Sample = (ref ActivityCreationOptions<ActivityContext> _) => ActivitySamplingResult.AllData
        };
        ActivitySource.AddActivityListener(listener);

        // Act
        using var activity = ActivitySourceProvider.StartActivity(
            serviceName,
            operationName,
            ActivityKind.Internal
        );

        // Assert
        activity.Should().NotBeNull();
        activity!.OperationName.Should().Be(operationName);
        activity.Kind.Should().Be(ActivityKind.Internal);
    }

    [Fact]
    public void AddTag_ShouldAddTagToCurrentActivity()
    {
        // Arrange
        var serviceName = "TestService";
        var operationName = "TestOperation";
        var tagKey = "test.key";
        var tagValue = "test.value";

        using var listener = new ActivityListener
        {
            ShouldListenTo = source => source.Name == serviceName,
            Sample = (ref ActivityCreationOptions<ActivityContext> _) => ActivitySamplingResult.AllData
        };
        ActivitySource.AddActivityListener(listener);

        // Act
        using var activity = ActivitySourceProvider.StartActivity(serviceName, operationName);
        ActivitySourceProvider.AddTag(tagKey, tagValue);

        // Assert
        activity.Should().NotBeNull();
        activity!.Tags.Should().Contain(tag => tag.Key == tagKey && tag.Value == tagValue);
    }

    [Fact]
    public void AddEvent_ShouldAddEventToCurrentActivity()
    {
        // Arrange
        var serviceName = "TestService";
        var operationName = "TestOperation";
        var eventName = "TestEvent";

        using var listener = new ActivityListener
        {
            ShouldListenTo = source => source.Name == serviceName,
            Sample = (ref ActivityCreationOptions<ActivityContext> _) => ActivitySamplingResult.AllData
        };
        ActivitySource.AddActivityListener(listener);

        // Act
        using var activity = ActivitySourceProvider.StartActivity(serviceName, operationName);
        ActivitySourceProvider.AddEvent(eventName, ("key1", "value1"), ("key2", 123));

        // Assert
        activity.Should().NotBeNull();
        activity!.Events.Should().Contain(e => e.Name == eventName);
    }

    [Fact]
    public void RecordException_ShouldSetErrorStatusAndRecordException()
    {
        // Arrange
        var serviceName = "TestService";
        var operationName = "TestOperation";
        var exception = new InvalidOperationException("Test exception");

        using var listener = new ActivityListener
        {
            ShouldListenTo = source => source.Name == serviceName,
            Sample = (ref ActivityCreationOptions<ActivityContext> _) => ActivitySamplingResult.AllData
        };
        ActivitySource.AddActivityListener(listener);

        // Act
        using var activity = ActivitySourceProvider.StartActivity(serviceName, operationName);
        ActivitySourceProvider.RecordException(exception);

        // Assert
        activity.Should().NotBeNull();
        activity!.Status.Should().Be(ActivityStatusCode.Error);
        activity.StatusDescription.Should().Be(exception.Message);
        activity.Events.Should().Contain(e => e.Name == "exception");
    }

    [Fact]
    public void AddTag_WithNoCurrentActivity_ShouldNotThrow()
    {
        // Arrange
        Activity.Current = null;

        // Act
        var act = () => ActivitySourceProvider.AddTag("test.key", "test.value");

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void AddEvent_WithNoCurrentActivity_ShouldNotThrow()
    {
        // Arrange
        Activity.Current = null;

        // Act
        var act = () => ActivitySourceProvider.AddEvent("TestEvent", ("key", "value"));

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void RecordException_WithNoCurrentActivity_ShouldNotThrow()
    {
        // Arrange
        Activity.Current = null;
        var exception = new InvalidOperationException("Test");

        // Act
        var act = () => ActivitySourceProvider.RecordException(exception);

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void StartActivity_WithDifferentKinds_ShouldCreateCorrectActivity()
    {
        // Arrange
        var serviceName = "TestService";
        var kinds = new[]
        {
            ActivityKind.Internal,
            ActivityKind.Server,
            ActivityKind.Client,
            ActivityKind.Producer,
            ActivityKind.Consumer
        };

        using var listener = new ActivityListener
        {
            ShouldListenTo = source => source.Name == serviceName,
            Sample = (ref ActivityCreationOptions<ActivityContext> _) => ActivitySamplingResult.AllData
        };
        ActivitySource.AddActivityListener(listener);

        // Act & Assert
        foreach (var kind in kinds)
        {
            using var activity = ActivitySourceProvider.StartActivity(
                serviceName,
                $"Operation_{kind}",
                kind
            );

            activity.Should().NotBeNull();
            activity!.Kind.Should().Be(kind);
        }
    }

    [Fact]
    public void AddTag_WithMultipleTags_ShouldNotThrow()
    {
        // Arrange
        var serviceName = "TestService";

        using var listener = new ActivityListener
        {
            ShouldListenTo = source => source.Name == serviceName,
            Sample = (ref ActivityCreationOptions<ActivityContext> _) => ActivitySamplingResult.AllData
        };
        ActivitySource.AddActivityListener(listener);

        // Act
        using var activity = ActivitySourceProvider.StartActivity(serviceName, "TestOperation");
        var act = () =>
        {
            ActivitySourceProvider.AddTag("tag1", "value1");
            ActivitySourceProvider.AddTag("tag2", 123);
            ActivitySourceProvider.AddTag("tag3", true);
        };

        // Assert
        act.Should().NotThrow();
        activity.Should().NotBeNull();
    }
}
