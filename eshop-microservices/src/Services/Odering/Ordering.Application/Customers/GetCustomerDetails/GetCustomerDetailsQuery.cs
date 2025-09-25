using Ordering.Application.Configuration.CQRS.Queries;

namespace Ordering.Application.Customers.GetCustomerDetails;

public class GetCustomerDetailsQuery : IQuery<CustomerDetailsDto>
{
    public Guid CustomerId { get; }
    public GetCustomerDetailsQuery(Guid customerId)
    {
        CustomerId = customerId;
    }
}
