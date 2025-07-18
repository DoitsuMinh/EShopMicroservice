using Ordering.Application.Configuration.CQRS.Commands;
using Ordering.Application.Configuration.Data;
using Ordering.Domain.Customers;
using Ordering.Domain.Customers.Orders;
using Ordering.Domain.ForeignExchange;
using Ordering.Domain.Products;
using Ordering.Domain.SeedWork;

namespace Ordering.Application.Orders.PlaceCustomerOrders;

public class PlaceCustomerOrderCommandHandler : ICommandHandler<PlaceCustomerOrderCommand, Guid>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly ISqlConnectionFactory _sqlConnectionFactory;
    private readonly IForeignExchange _foreignExchange;
    private readonly IUnitOfWork _uow;
    public PlaceCustomerOrderCommandHandler(ICustomerRepository customerRepository, ISqlConnectionFactory sqlConnectionFactory, IForeignExchange foreignExchange, IUnitOfWork uow)
    {
        _customerRepository = customerRepository;
        _sqlConnectionFactory = sqlConnectionFactory;
        _foreignExchange = foreignExchange;
        _uow = uow;
    }

    public async Task<Guid> Handle(PlaceCustomerOrderCommand command, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetByIdAsync(new CustomerId(command.CustomerId));
        
        var allProductPrices = await ProductPriceProvider.GetAllProductPricesAsync(_sqlConnectionFactory.GetOpenConnection());

        var conversionRates = await _foreignExchange.GetConversionRatesAsync();

        var orderProductsData = command.Products.Select(x => new OrderProductData(new ProductId(x.Id), x.Quantity)).ToList();

        var orderId = customer.PlacedOrder(
            orderProductsData,
            allProductPrices,
            command.Currency,
            conversionRates);

        await _uow.CommitAsync();

        return orderId.Value;
    }
}
