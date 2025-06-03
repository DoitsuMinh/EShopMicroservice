using Ordering.Application.Configuration.Validation;

namespace Ordering.API.SeedWork;

internal class InvalidCommandProblemDetails: Microsoft.AspNetCore.Mvc.ProblemDetails
{
    public InvalidCommandProblemDetails(InvalidCommandException exception)
    {
        Title = "Invalid Command";
        Detail = exception.Details;
        Status = StatusCodes.Status400BadRequest; // Bad Request
        Type = "https://example.com/probs/invalid-command";
    }
}
