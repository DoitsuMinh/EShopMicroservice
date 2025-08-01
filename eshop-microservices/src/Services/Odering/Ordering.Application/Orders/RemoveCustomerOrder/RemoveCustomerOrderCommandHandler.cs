using MediatR;
using Ordering.Application.Configuration.CQRS.Commands;
using Ordering.Domain.Customers;
using Ordering.Domain.Customers.Orders;

namespace Ordering.Application.Orders.RemoveCustomerOrder;

public class RemoveCustomerOrderCommandHandler : ICommandHandler<RemoveCustomerOrderCommand, Unit>
{
    private readonly ICustomerRepository _customerRepository;

    public RemoveCustomerOrderCommandHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<Unit> Handle(RemoveCustomerOrderCommand request, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetByIdAsync(new CustomerId(request.CustomerId));

        customer.RemoveOrder(new OrderId(request.OrderId));

        return Unit.Value;
    }
}
