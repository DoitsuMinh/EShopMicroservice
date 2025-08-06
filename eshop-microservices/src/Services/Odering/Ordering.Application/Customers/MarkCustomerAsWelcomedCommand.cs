using MediatR;
using Ordering.Application.Configuration.CQRS.Commands;
using Ordering.Domain.Customers;
using System.Text.Json.Serialization;

namespace Ordering.Application.Customers;

public class MarkCustomerAsWelcomedCommand : InternalCommandBase<Unit>
{
    [JsonConstructor]
    public MarkCustomerAsWelcomedCommand(Guid id, CustomerId customerId)
        : base(id)
    {
        CustomerId = customerId;
    }
    public CustomerId CustomerId { get; }
}
