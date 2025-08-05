using Microsoft.AspNetCore.Mvc;
using Ordering.Domain.SeedWork;

namespace Ordering.API.SeedWork;

public class EntityNotFoundProblemDetails : ProblemDetails
{
    public EntityNotFoundProblemDetails(NotFoundException exception)
    {
        Title = nameof(NotFoundException);
        Detail = exception.Message;
        Status = StatusCodes.Status404NotFound;
        Type = "https://example.com/probs/not-found";
    }
}
