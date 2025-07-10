using Ordering.Application.Configuration.CQRS.Commands;
using Ordering.Application.Configuration.Data;
using Ordering.Domain.Customers;
using Ordering.Domain.ForeignExchange;

namespace Ordering.Application.Orders.PlaceCustomerOrders;

public class PlaceCustomerOrderCommandHandler : ICommandHandler<PlaceCustomerOrderCommand, Guid>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly ISqlConnectionFactory _sqlConnectionFactory;
    private readonly IForeignExchange _foreignExchange;

    public PlaceCustomerOrderCommandHandler(ICustomerRepository customerRepository, ISqlConnectionFactory sqlConnectionFactory, IForeignExchange foreignExchange)
    {
        _customerRepository = customerRepository;
        _sqlConnectionFactory = sqlConnectionFactory;
        _foreignExchange = foreignExchange;
    }

    public Task<Guid> Handle(PlaceCustomerOrderCommand request, CancellationToken cancellationToken)
    {
        var customer = _customerRepository.GetByIdAsync(new CustomerId(request.CustomerId));
        throw new NotImplementedException();
    }
}
