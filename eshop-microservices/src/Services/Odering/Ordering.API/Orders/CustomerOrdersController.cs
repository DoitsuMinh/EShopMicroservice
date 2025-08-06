using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Orders.ChangeCustomerOrder;
using Ordering.Application.Orders.GetCustomerOrderDetails;
using Ordering.Application.Orders.GetCustomerOrders;
using Ordering.Application.Orders.PlaceCustomerOrders;
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
    public async Task<ActionResult<List<OrderDto>>> GetCustomerOrdes(Guid customerId)
    {
        var customerOrders = await _mediator.Send(new GetCustomerOrdersQuery(customerId));
        return Ok(customerOrders);
    }

    /// <summary>
    /// Retrieves the details of a specific order for a customer by order ID.
    /// </summary>
    [Route("{customerId:guid}/orders/{orderId:guid}")]
    [HttpGet]
    [ProducesResponseType(typeof(OrderDetailsDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<OrderDetailsDto>> GetCustomerOrderDetails(
        [FromRoute] Guid orderId)
    {
        var orderDetails = await _mediator.Send(new GetCustomerOrderDetailsQuery(orderId));
        return Ok(orderDetails);
    }

    /// <summary>
    /// adds a new order for a specific customer.
    /// </summary>
    [Route("{customerId:guid}/orders")]
    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    public async Task<IActionResult> AddCustomerOrder(
        [FromRoute] Guid customerId,
        [FromBody] CustomerOrderRequest request)
    {
        var id = await _mediator.Send(new PlaceCustomerOrderCommand(customerId, request.Products, request.Currency));
        return Created(string.Empty, id);
    }

    [Route("{customerId:guid}/orders/{orderId:guid}")]
    [HttpPut]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<IActionResult> ChangeCustomerOrder(
        [FromRoute] Guid customerId,
        [FromRoute] Guid orderId,
        [FromBody] CustomerOrderRequest request)
    {
        await _mediator.Send(new ChangeCustomerOrderCommand(customerId, orderId, request.Products, request.Currency));
        return Ok();
    }
}
