using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Orders.GetCustomerOrders;
using System.Net;

namespace Ordering.API.Orders;

/// <summary>
/// Handles order-related operations.
/// </summary>
[Route("api/customers")]
[ApiController]
public class CustomerOrdersController : Controller
{
    private readonly IMediator _mediator;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public CustomerOrdersController(IMediator mediator)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Retrieves the orders for a specific customer by their ID.
    /// </summary>
    [Route("{customerId:guid}/orders")]
    [HttpGet]
    [ProducesResponseType(typeof(List<OrderDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCustomerOrdes(Guid customerId)
    {
        var customerOrders = await _mediator.Send(new GetCustomerOrdersQuery(customerId));
        return Ok(customerOrders);
    }

    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    public async Task<IActionResult> AddCustomerOrder(
        [FromRoute] Guid customerId,
        [FromBody] CustomerOrderRequest request)
    {
        await _mediator.Send(new PlaceCustomerOrderCommand());

        return Created(string.Empty, null);
    }
}
