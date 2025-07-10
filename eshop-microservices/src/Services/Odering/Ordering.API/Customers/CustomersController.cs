using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Customers;
using Ordering.Application.Customers.GetCustomerDetails;
using Ordering.Application.Customers.RegisterCustomer;

namespace Ordering.API.Customers;

/// <summary>  
/// Handles customer-related operations.
/// </summary>  
[Route("api/customers")]
[ApiController]
public class CustomersController : Controller
{
    private readonly IMediator _mediator;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public CustomersController(IMediator mediator)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    /// <summary>  
    /// Gets customer details ID.  
    /// </summary>  
    /// <param name="customerId"></param>
    [Route("{customerId:guid}")]
    [HttpGet]
    [ProducesResponseType(typeof(CustomerDetailsDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<CustomerDetailsDto>> GetCustomer(Guid customerId)
    {
        var customer = await _mediator.Send(new GetCustomerDetailsQuery(customerId));

        if (customer == null)
        {
            return NotFound();
        }
        return Ok(customer);
    }

    /// <summary>
    /// Register new customer.
    /// </summary>
    /// <param name="request"></param>
    [HttpPost("")]
    [ProducesResponseType(typeof(CustomerDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateCustomer([FromBody] RegisterCustomerRequest request)
    {
        var customer = await _mediator.Send(new RegisterCustomerCommand(request.Email, request.Name));
        return Created(string.Empty, customer);
    }
}
