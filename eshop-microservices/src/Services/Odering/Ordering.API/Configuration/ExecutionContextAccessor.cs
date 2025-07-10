using Ordering.Application;

namespace Ordering.API.Configuration;

/// <summary>
/// ExecutionContextAccessor is used to access the execution context of the current HTTP request.
/// </summary>
public class ExecutionContextAccessor : IExecutionContextAccessor
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExecutionContextAccessor"/> class.
    /// </summary>
    public ExecutionContextAccessor(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    /// <summary>
    /// Gets the correlation ID from the HTTP request headers.
    /// </summary>
    public Guid CorrelationId
    {
        get
        {
            if (IsAvailable && _httpContextAccessor.HttpContext.Request.Headers.Keys.Any(x => x == CorrelationMiddleware.CorrelationHeaderKey))
            {
                return Guid.Parse(_httpContextAccessor.HttpContext.Request.Headers[CorrelationMiddleware.CorrelationHeaderKey]);
            }
            throw new ApplicationException("Http context and correlation id is not available");
        }
    }

    public bool IsAvailable => _httpContextAccessor.HttpContext != null;
}