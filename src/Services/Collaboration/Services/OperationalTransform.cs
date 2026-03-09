namespace Collaboration.Service.Services;

/// <summary>
/// Operational Transformation for concurrent text editing
/// Implements OT algorithm to handle concurrent edits from multiple users
/// Validates: Requirement 32.2
/// </summary>
public class OperationalTransform
{
    /// <summary>
    /// Represents a text operation (insert or delete)
    /// </summary>
    public class Operation
    {
        public OperationType Type { get; set; }
        public int Position { get; set; }
        public string? Text { get; set; }
        public int Length { get; set; }
        public string UserId { get; set; } = string.Empty;
        public long Version { get; set; }
    }

    public enum OperationType
    {
        Insert,
        Delete,
        Retain
    }

    /// <summary>
    /// Transform operation A against operation B
    /// Returns transformed operation A' that can be applied after B
    /// </summary>
    public static Operation Transform(Operation opA, Operation opB)
    {
        var transformedOp = new Operation
        {
            Type = opA.Type,
            Position = opA.Position,
            Text = opA.Text,
            Length = opA.Length,
            UserId = opA.UserId,
            Version = opA.Version + 1
        };

        // If operations are from the same user, no transformation needed
        if (opA.UserId == opB.UserId)
        {
            return transformedOp;
        }

        // Transform based on operation types
        if (opB.Type == OperationType.Insert)
        {
            // If B inserts before A's position, shift A's position right
            if (opB.Position <= opA.Position)
            {
                transformedOp.Position += opB.Text?.Length ?? 0;
            }
        }
        else if (opB.Type == OperationType.Delete)
        {
            // If B deletes before A's position, shift A's position left
            if (opB.Position < opA.Position)
            {
                var deleteEnd = opB.Position + opB.Length;
                if (deleteEnd <= opA.Position)
                {
                    // Delete is entirely before A
                    transformedOp.Position -= opB.Length;
                }
                else
                {
                    // Delete overlaps with A's position
                    transformedOp.Position = opB.Position;
                }
            }
            else if (opB.Position < opA.Position + (opA.Type == OperationType.Insert ? (opA.Text?.Length ?? 0) : opA.Length))
            {
                // B deletes within A's range
                if (opA.Type == OperationType.Delete)
                {
                    // Both are deletes - adjust length
                    var overlap = Math.Min(opB.Position + opB.Length, opA.Position + opA.Length) - Math.Max(opB.Position, opA.Position);
                    if (overlap > 0)
                    {
                        transformedOp.Length -= overlap;
                    }
                }
            }
        }

        return transformedOp;
    }

    /// <summary>
    /// Apply an operation to text
    /// </summary>
    public static string ApplyOperation(string text, Operation op)
    {
        switch (op.Type)
        {
            case OperationType.Insert:
                if (op.Position > text.Length)
                {
                    op.Position = text.Length;
                }
                return text.Insert(op.Position, op.Text ?? string.Empty);

            case OperationType.Delete:
                if (op.Position >= text.Length)
                {
                    return text;
                }
                var deleteLength = Math.Min(op.Length, text.Length - op.Position);
                return text.Remove(op.Position, deleteLength);

            case OperationType.Retain:
                return text;

            default:
                return text;
        }
    }

    /// <summary>
    /// Transform a list of operations against a base operation
    /// </summary>
    public static List<Operation> TransformOperations(List<Operation> operations, Operation baseOp)
    {
        var transformed = new List<Operation>();
        foreach (var op in operations)
        {
            transformed.Add(Transform(op, baseOp));
        }
        return transformed;
    }

    /// <summary>
    /// Compose two operations into a single operation
    /// </summary>
    public static Operation? Compose(Operation op1, Operation op2)
    {
        // If operations are adjacent and of the same type, compose them
        if (op1.Type == op2.Type && op1.UserId == op2.UserId)
        {
            if (op1.Type == OperationType.Insert)
            {
                if (op1.Position + (op1.Text?.Length ?? 0) == op2.Position)
                {
                    return new Operation
                    {
                        Type = OperationType.Insert,
                        Position = op1.Position,
                        Text = op1.Text + op2.Text,
                        UserId = op1.UserId,
                        Version = Math.Max(op1.Version, op2.Version)
                    };
                }
            }
            else if (op1.Type == OperationType.Delete)
            {
                if (op1.Position == op2.Position)
                {
                    return new Operation
                    {
                        Type = OperationType.Delete,
                        Position = op1.Position,
                        Length = op1.Length + op2.Length,
                        UserId = op1.UserId,
                        Version = Math.Max(op1.Version, op2.Version)
                    };
                }
            }
        }

        return null;
    }
}
