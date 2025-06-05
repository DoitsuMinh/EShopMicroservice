namespace Ordering.Application;

public interface IExecutionContextAccessor
{
    Guid CorrelationId { get; }
    bool IsAvailable { get; }
}
