using Ordering.Domain.SeedWork;

namespace Ordering.API.SeedWork;

/// <summary>
/// Maps <see cref="BusinessRuleValidationException"/> to a ProblemDetails response.
/// </summary>
public class BusinessRuleValidationExceptionProblemDetails: Microsoft.AspNetCore.Mvc.ProblemDetails
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BusinessRuleValidationExceptionProblemDetails"/> class
    /// </summary>
    public BusinessRuleValidationExceptionProblemDetails(BusinessRuleValidationException exception)
    {
        Title = "Business rule validation error";
        Detail = exception.Details;
        Status = StatusCodes.Status409Conflict; // Bad Request
        Type = "https://example.com/probs/business-rule-validation-error";
    }
}
