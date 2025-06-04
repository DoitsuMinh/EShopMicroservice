namespace Ordering.Domain.Customers;

public interface ICustomerRepository
{
    Task<Customer> GetByIdAsync(CustomerId id);
    Task AddAsync(Customer customer);
}
