namespace Shared.Exceptions;

/// <summary>
/// Exception thrown when a requested resource is not found
/// </summary>
public class NotFoundException : DomainException
{
    public string ResourceType { get; }
    public object ResourceId { get; }

    public NotFoundException(string resourceType, object resourceId)
        : base($"{resourceType} with id '{resourceId}' was not found", "RESOURCE_NOT_FOUND")
    {
        ResourceType = resourceType;
        ResourceId = resourceId;
        AddMetadata("resourceType", resourceType);
        AddMetadata("resourceId", resourceId);
    }

    public NotFoundException(string message, string resourceType, object resourceId)
        : base(message, "RESOURCE_NOT_FOUND")
    {
        ResourceType = resourceType;
        ResourceId = resourceId;
        AddMetadata("resourceType", resourceType);
        AddMetadata("resourceId", resourceId);
    }
}
