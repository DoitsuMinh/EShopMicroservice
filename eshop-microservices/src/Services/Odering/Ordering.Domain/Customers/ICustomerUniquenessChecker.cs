namespace Ordering.Domain.Customers;

public interface ICustomerUniquenessChecker
{
    bool IsUnique(string email);
}