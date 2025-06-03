using MediatR;

namespace Ordering.Domain.SeedWork;

public interface IDomainEvent: INotification
{
    DateTime OccuredOn { get; }
}
