using Odering.Infrastructure.Database;
using Ordering.Domain.Customers;

namespace Odering.Infrastructure.Customers;

public class CustomerRepository : ICustomerRepository
{
    private readonly OrdersContext _ordersContext;

    public CustomerRepository(OrdersContext ordersContext)
    {
        _ordersContext = ordersContext ?? throw new ArgumentNullException(nameof(ordersContext));
    }

    public async Task AddAsync(Customer customer)
    {
        await _ordersContext.Customers.AddAsync(customer);
    }

    public Task<Customer> GetByIdAsync(CustomerId id)
    {
        throw new NotImplementedException();
    }
}
