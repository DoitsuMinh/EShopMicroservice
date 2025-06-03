namespace Ordering.API.Configuration;

internal class CorrelationMiddleware
{
    internal const string CorrelationHeaderKey = "CorrelationId";

    private readonly RequestDelegate _next;

    public CorrelationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var correlationId = Guid.NewGuid();

        if (context.Request is not null)
        {
            context.Request.Headers.Append(CorrelationHeaderKey, correlationId.ToString());
        }

        await _next(context);
    }
}
