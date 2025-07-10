using Microsoft.AspNetCore.Mvc;
using Ordering.Domain.SeedWork;
using System;

namespace Ordering.API.SeedWork;

public class EntityNotFoundProblemDetails : ProblemDetails
{
    public EntityNotFoundProblemDetails(EntityNotFoundException exception)
    {
        Title = "";
        Detail = exception.Message;
        Status = StatusCodes.Status404NotFound;
        Type = "https://example.com/probs/not-found";
    }
}
