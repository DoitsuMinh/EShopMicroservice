using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Customers.GetCustomerDetails;

namespace Ordering.API.Customers;

/// <summary>  
/// Handles customer-related operations.
/// </summary>  
[Route("api/customers")]
[ApiController]
public class CustomersController : Controller
{
    private readonly IMediator _mediator;

    public CustomersController(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    /// <summary>  
    /// Gets customer details ID.  
    /// </summary>  
    /// <param name="customerId"></param>
    [Route("{customerId:guid}")]
    [HttpGet]
    public async Task<IActionResult> GetCustomer(Guid customerId)
    {
        var result = await _mediator.Send(new GetCustomerDetailsQuery(customerId));

        return Ok(result);
    }
}
