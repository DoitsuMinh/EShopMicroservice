namespace Ordering.Application.Customers.GetCustomerDetails;

public class CustomerDetailsDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = default!;

    public string Email { get; set; } = default!;

    public bool WelcomeEmailWasSent { get; set; }
}
