using Microsoft.AspNetCore.Mvc;

namespace Ordering.API.SeedWork;

public class EntityNotFoundProblemDetails : ProblemDetails
{
    public EntityNotFoundProblemDetails(Exception exception)
    {
        Title = "Not Found";
        Detail = exception.Message;
        Status = StatusCodes.Status404NotFound;
        Type = "https://example.com/probs/not-found";
    }
}
