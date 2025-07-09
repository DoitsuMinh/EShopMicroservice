using Ordering.Application.Configuration.CQRS.Commands;

namespace Ordering.Application.Customers.RegisterCustomer;

public class RegisterCustomerCommand : CommandBase<CustomerDto>
{
    public string Email { get; set; }
    public string Name { get; set; }

    public RegisterCustomerCommand(string email, string name)
    {
        Email = email;
        Name = name;
    }
}
