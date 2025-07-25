﻿using Autofac;
using Odering.Infrastructure.Domain.ForeignExchanges;
using Ordering.Application.Customers.DomainService;
using Ordering.Domain.Customers;
using Ordering.Domain.ForeignExchange;

namespace Odering.Infrastructure.Domain;

public class DomainModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        // Register domain services, repositories, and other domain-related components here.
        // For example:
        // builder.RegisterType<SomeDomainService>().As<ISomeDomainService>().InstancePerLifetimeScope();

        // You can also register domain events, factories, or any other domain-specific logic.

        // Example of registering a repository:
        // builder.RegisterType<SomeRepository>().As<ISomeRepository>().InstancePerLifetimeScope();
        builder.RegisterType<CustomerUniquenessChecker>()
            .As<ICustomerUniquenessChecker>()
            .InstancePerLifetimeScope();

        builder.RegisterType<ForeignExchange>()
            .As<IForeignExchange>()
            .InstancePerLifetimeScope();
    }
}