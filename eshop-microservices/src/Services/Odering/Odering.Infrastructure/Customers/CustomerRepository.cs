using Odering.Infrastructure.Database;
using Ordering.Domain.Customers;
using Ordering.Domain.SeedWork;

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

    public async Task<Customer> GetByIdAsync(CustomerId id)
    {
        return await _ordersContext.Customers
            .Include(c => c.Orders)
            .FindAsync(id)
            ?? throw new EntityNotFoundException($"Customer with ID {id.Value} not found.");
    }
}
