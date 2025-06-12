using Ordering.Application.Configuration.Queries;

namespace Ordering.Application.Customers.GetCustomerDetails;

public class GetCustomerDetailsQuery : IQuery<CustomerDto>
{
    public Guid CustomerId { get; }
    public GetCustomerDetailsQuery(Guid customerId)
    {
        CustomerId = customerId;
    }
}
