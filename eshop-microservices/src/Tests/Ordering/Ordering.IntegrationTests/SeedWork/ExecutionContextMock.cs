
using Ordering.Application;

namespace Ordering.IntegrationTests.SeedWork;

public class ExecutionContextMock : IExecutionContextAccessor
{
    public Guid CorrelationId { get; set; }

    public bool IsAvailable { get; set; }
}