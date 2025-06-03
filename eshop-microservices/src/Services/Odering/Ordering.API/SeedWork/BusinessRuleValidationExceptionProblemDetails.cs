using Ordering.Domain.SeedWork;

namespace Ordering.API.SeedWork;

public class BusinessRuleValidationExceptionProblemDetails: Microsoft.AspNetCore.Mvc.ProblemDetails
{
    public BusinessRuleValidationExceptionProblemDetails(BusinessRuleValidationException exception)
    {
        Title = "Business rult validation error";
        Detail = exception.Details;
        Status = StatusCodes.Status409Conflict; // Bad Request
        Type = "https://example.com/probs/business-rule-validation-error";
    }
}
