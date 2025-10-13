using BuildlingBlocks.Messaging.Events;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BuildlingBlocks.Messaging.MassTransit;

public static class Extensions
{
    public static IServiceCollection AddMessageBroker(
        this IServiceCollection services, 
        IConfiguration configuration, 
        Assembly? assembly = null)
    {
        services.AddMassTransit(config =>
        {
            /* Ex:
             * Default      ->  SubmitOrder
             * Snake Case   ->  submit_order
             * Kebab Case   ->  submit-order
             */
            config.SetKebabCaseEndpointNameFormatter();

            // Register the consumers from the specified assembly
            /*
             * assembly: Ordering.Application
             * Consumer: BasketCheckoutEventHandler
             * Endpoint: basket-checkout-event-handler
             * Queue: basket-checkout-event-handler
             * Message: BasketCheckoutEvent
             * Routing Key: basket-checkout-event
             * Exchange: basket-checkout-event
             * Binding: basket-checkout-event -> basket-checkout-event-handler
             * Message Type: BasketCheckoutEvent
             * Handler Method: Consume
             * Message Body: BasketCheckoutEvent
             */
            if (assembly is not null)
            {
                config.AddConsumers(assembly);
            }

            //console log MessageBroker settings
            Console.WriteLine("MessageBroker Settings:");
            Console.WriteLine($"MessageBrokerHost: {configuration["MessageBroker:Host"]}");
            Console.WriteLine($"MessageBrokerUserName: {configuration["MessageBroker:UserName"]}");


            config.UsingRabbitMq((context, configurator) =>
            {
                configurator.Host(new Uri(configuration["MessageBroker:Host"]!), host =>
                {
                    host.Username(configuration["MessageBroker:UserName"]);
                    host.Password(configuration["MessageBroker:Password"]);
                });

                configurator.ConfigureEndpoints(context);

                /* Uncomment the following to MassTransit is working
                 */                
                //configurator.ReceiveEndpoint("test-queue", endpoint =>
                //{
                //    endpoint.Handler<BasketCheckoutEvent>(async context =>
                //    {
                //        await Task.Yield();
                //        Console.WriteLine("Test consumer triggered");
                //    });
                //});
            });
        });

        return services;
    }
}
