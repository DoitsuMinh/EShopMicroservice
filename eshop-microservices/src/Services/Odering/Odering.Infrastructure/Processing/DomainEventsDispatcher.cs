using Autofac;
using Autofac.Core;
using MediatR;
using Newtonsoft.Json;
using Odering.Infrastructure.Database;
using Odering.Infrastructure.Processing.Outbox;
using Ordering.Application.Configuration.DomainEvents;
using Ordering.Domain.SeedWork;

namespace Odering.Infrastructure.Processing;

public class DomainEventsDispatcher : IDomainEventsDispatcher
{
    private readonly IMediator _mediator;
    private readonly OrdersContext _ordersContext;
    private readonly ILifetimeScope _scope;

    public DomainEventsDispatcher(IMediator mediator, OrdersContext ordersContext, ILifetimeScope scope)
    {
        _mediator = mediator;
        _ordersContext = ordersContext;
        _scope = scope;
    }

    public async Task DispatchEventsAsync()
    {
        // Retrieve all entities tracked by the context that have domain events  
        var domainEntities =
            _ordersContext.ChangeTracker
            .Entries<Entity>()
            .Where(e => e.Entity.DomainEvents != null && e.Entity.DomainEvents.Any())
            .ToList();

        // Extract all domain events from the entities  
        var domainEvents =
            domainEntities
            .SelectMany(de => de.Entity.DomainEvents)
            .ToList();

        // Prepare a list to hold domain event notifications (if needed for further processing)  
        var domainEventNotifications = new List<IDomainEventNotification<IDomainEvent>>();

        // Iterate through each domain event to resolve its corresponding notification type  
        foreach (var domainEvent in domainEvents)
        {
            // Dynamically create the notification type for the current domain event  
            Type domainEventNotificationType = typeof(IDomainEventNotification<>);
            var domainNotificationWithGenericType = domainEventNotificationType.MakeGenericType(domainEvent.GetType());

            // Resolve the notification instance from the Autofac container  
            var domainNotification = _scope.ResolveOptional(domainNotificationWithGenericType, new List<Parameter>
            {
                    new NamedParameter("domainEvent", domainEvent)
            });
            if (domainNotification != null)
            {
                domainEventNotifications.Add((IDomainEventNotification<IDomainEvent>)domainNotification);
            }
        }

        // Clear domain events from the entities to prevent re-processing  
        domainEntities.ForEach(entity => entity.Entity.ClearDomainEvents());

        // Publish all domain events using the mediator  
        var tasks = domainEvents.Select(async (domainEvent) =>
        {
            await _mediator.Publish(domainEvent);
        });

        // Wait for all publishing tasks to complete  
        await Task.WhenAll(tasks);

        foreach (var domainEventNotification in domainEventNotifications)
        {
            string type = domainEventNotification.GetType().FullName ?? string.Empty;
            var data  = JsonConvert.SerializeObject(domainEventNotification);

            var outBoxMessage = new OutboxMessage(
                domainEventNotification.DomainEvent.OccuredOn,
                type,
                data);

            _ordersContext.OutboxMessages.Add(outBoxMessage);
        }
    }
}