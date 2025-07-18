using Autofac;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Odering.Infrastructure.Customers;
using Odering.Infrastructure.Domain;
using Odering.Infrastructure.Domain.Products;
using Odering.Infrastructure.SeedWork;
using Ordering.Application.Configuration.Data;
using Ordering.Domain.Customers;
using Ordering.Domain.Products;
using Ordering.Domain.SeedWork;

namespace Odering.Infrastructure.Database;

/// <summary>
/// Autofac module for registering data access related services and DbContext.
/// </summary>
public class DataAccessModule : Module
{
    private readonly string _connectionString;

    /// <summary>
    /// Initializes the module with the database connection string.
    /// </summary>
    /// <param name="connectionString">The database connection string.</param>
    public DataAccessModule(string connectionString)
    {
        _connectionString = connectionString;
    }

    /// <summary>
    /// Registers data access services and DbContext in the Autofac container.
    /// </summary>
    /// <param name="builder">The Autofac container builder.</param>
    protected override void Load(ContainerBuilder builder)
    {
        // Register the SQL connection factory with the provided connection string.
        builder.RegisterType<SqlConnectionFactory>()
            .As<ISqlConnectionFactory>()
            .WithParameter("connectionString", _connectionString)
            .InstancePerLifetimeScope();

        // Register UnitOfWork as a transient service, allowing it to be created per request.
        builder.RegisterType<UnitOfWork>()
            .As<IUnitOfWork>()
            .InstancePerLifetimeScope();

        // Register ICustomerrepository as a transient service, allowing it to be created per request.
        builder.RegisterType<CustomerRepository>()
            .As<ICustomerRepository>()
            .InstancePerLifetimeScope();

        // Register IProductRepository as a transient service, allowing it to be created per request.
        builder.RegisterType<ProductRepository>()
            .As<IProductRepository>()
            .InstancePerLifetimeScope();

        // Register the StronglyTypedIdValueConverterSelector to handle strongly-typed IDs in EF Core.
        builder.RegisterType<StronglyTypedIdValueConverterSelector>()
            .As<IValueConverterSelector>()
            .InstancePerLifetimeScope();

        // Register the OrdersContext DbContext with custom options.
        builder
            .Register(c =>
            {
                // Configure DbContext options to use Npgsql and replace the value converter selector.
                var optionsBuilder = new DbContextOptionsBuilder<OrdersContext>();
                optionsBuilder.UseNpgsql(_connectionString);

                // Replace the default value converter selector with a custom one for strongly-typed IDs.
                optionsBuilder.ReplaceService<IValueConverterSelector, StronglyTypedIdValueConverterSelector>();

                // Create and return the OrdersContext instance.
                return new OrdersContext(optionsBuilder.Options);
            })
            .AsSelf() // Register the DbContext as itself
            .As<DbContext>()    // Register as DbContext for compatibility with other EF Core services
            .InstancePerLifetimeScope();
    }
}
