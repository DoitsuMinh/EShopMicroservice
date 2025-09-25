namespace Ordering.Application.Customers.RegisterCustomer;

public class RegisterCustomerRequest
{
    public string Email { get; set; } = default!;
    public string Name { get; set; } = default!;
}
