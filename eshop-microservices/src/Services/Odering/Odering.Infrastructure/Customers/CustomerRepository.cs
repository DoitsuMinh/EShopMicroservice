using Microsoft.EntityFrameworkCore;
using Odering.Infrastructure.Database;
using Odering.Infrastructure.Domain.Customers;
using Odering.Infrastructure.SeedWork;
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
        /// Get customer by ID with eager loading of orders and products
        try
        {
            return await _ordersContext.Customers
            .IncludePaths(
                CustomerEntityTypeConfiguration.OrderList, 
                CustomerEntityTypeConfiguration.OrderProducts)
            .SingleAsync(c => c.Id == id) ?? throw new EntityNotFoundException(nameof(Customer), id.Value);
        }
        catch (Exception ex)
        {

            throw new Exception(ex.Message);
        }

    }
}
