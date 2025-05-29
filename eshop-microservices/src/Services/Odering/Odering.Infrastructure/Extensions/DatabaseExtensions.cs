using Dapper;
using FluentMigrator.Runner;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using Npgsql;
using Odering.Infrastructure.DataMapping;

namespace Odering.Infrastructure.Extensions;

public static class DatabaseExtensions
{
    public static async Task InitialiseDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var migrator = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
        var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>()
                                .CreateLogger("Migraion");

        // Run migrations asynchronously
        await Task.Run(() =>
        {
            try
            {
                logger.LogInformation("Starting database migration...");
                migrator.MigrateUp(); // Apply migrations
                // log finish migration
                logger.LogInformation("Finshed migration");
            }
            catch (Exception ex)
            {
                // Handle exceptions during migration
                logger.LogError("Migration failed: {ErrorMessage}", ex.Message);
                migrator.Rollback(0);
            }
        });
    }

    // Function to Seed Initial Data Asynchronously using Dapper with input is connection string
    public static async Task SeedInitialDataAsync(this WebApplication app, string connectionString)
    {
        ArgumentNullException.ThrowIfNull(connectionString);
        using var scope = app.Services.CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>()
                                  .CreateLogger("Seeding");
        using (var connection = new NpgsqlConnection(connectionString))
        {
            // TODO
            // use Serilog for logging

            await connection.OpenAsync();
            logger.LogInformation("Starting transaction...");

            using var transaction = await connection.BeginTransactionAsync();
            try
            {
                // Handle Customer seeding  
                var existsCustomerSql = @"SELECT EXISTS(SELECT 1 FROM public.""Customer"" c WHERE c.""Id"" > 0 LIMIT 1);";
                var existsCustomerow = await connection.ExecuteScalarAsync<bool>(existsCustomerSql);
                if (existsCustomerow)
                {
                    logger.LogInformation("Customers already exist in the database. Skipping seeding.");
                }
                else
                {
                    var customerSql = @"INSERT INTO public.""Customer"" (""Name"", ""Email"") VALUES (@Name, @Email)";
                    var customers = InitialData.Customers.ToList();
                    var rowsCustomerAffected = await connection.ExecuteAsync(customerSql, customers);
                    if (rowsCustomerAffected == 0)
                    {
                        throw new Exception("No dummy customers were seeded into the database.");
                    }
                    logger.LogInformation("Seeded {RowsCustomerAffected} customers into the database.", rowsCustomerAffected);
                }

                // Handle Product seeding  
                var existsProductSql = @"SELECT EXISTS(SELECT 1 FROM public.""Products"" p WHERE p.""Id"" > 0 LIMIT 1);";
                var existsProductRow = await connection.ExecuteScalarAsync<bool>(existsProductSql);
                if (existsProductRow)
                {
                    logger.LogInformation("Products already exist in the database. Skipping seeding.");
                }
                else
                {
                    var productSql = @"INSERT INTO public.""Products"" (""Name"", ""Price"") VALUES (@Name, @Price)";
                    var products = InitialData.Products.ToList();
                    var rowsProductsAffected = await connection.ExecuteAsync(productSql, products);
                    if (rowsProductsAffected == 0)
                    {
                        throw new Exception("No dummy products were seeded into the database.");
                    }
                    logger.LogInformation("Seeded {RowsProductsAffected} products into the database.", rowsProductsAffected);
                }

                // Handle Order seeding  
                var existsOrderSql = @"SELECT EXISTS(SELECT 1 FROM public.""Orders"" p WHERE p.""Id"" > 0 LIMIT 1);";
                var existsOrderRow = await connection.ExecuteScalarAsync<bool>(existsOrderSql);
                if (existsOrderRow)
                {
                    logger.LogInformation("Orders already exist in the database. Skipping seeding.");
                }
                else
                {
                    var orderSql = @"
                                    INSERT INTO public.""Orders"" 
                                   (
                                    ""CustomerId"",
                                    ""TotalPrice"",
                                    ""BillingAddress_AddressLine"",
                                    ""BillingAddress_Country"",
                                    ""BillingAddress_EmailAddress"",
                                    ""BillingAddress_FirstName"",
                                    ""BillingAddress_LastName"",
                                    ""BillingAddress_State"",
                                    ""BillingAddress_ZipCode"",
                                    ""OrderName"",
                                    ""Payment_CVV"",
                                    ""Payment_CardName"",
                                    ""Payment_CardNumber"",
                                    ""Payment_Expiration"",
                                    ""Payment_PaymentMethod"",
                                    ""ShippingAddress_AddressLine"",
                                    ""ShippingAddress_Country"",
                                    ""ShippingAddress_EmailAddress"",
                                    ""ShippingAddress_FirstName"",
                                    ""ShippingAddress_LastName"",
                                    ""ShippingAddress_State"",
                                    ""ShippingAddress_ZipCode""
                                   ) VALUES 
                                   (
                                    @CustomerId,
                                    @TotalPrice,
                                    @BillingAddress_AddressLine,
                                    @BillingAddress_Country,
                                    @BillingAddress_EmailAddress,
                                    @BillingAddress_FirstName,
                                    @BillingAddress_LastName,
                                    @BillingAddress_State,
                                    @BillingAddress_ZipCode,
                                    @OrderName,
                                    @Payment_CVV,
                                    @Payment_CardName,
                                    @Payment_CardNumber,
                                    @Payment_Expiration,
                                    @Payment_PaymentMethod,
                                    @ShippingAddress_AddressLine,
                                    @ShippingAddress_Country,
                                    @ShippingAddress_EmailAddress,
                                    @ShippingAddress_FirstName,
                                    @ShippingAddress_LastName,
                                    @ShippingAddress_State,
                                    @ShippingAddress_ZipCode
                                   )";
                    
                    var orders = InitialData.OrdersWithItems.ToList();

                    var insertedOrderValues = orders.Select(o => InsertedOrderValue.MapFrom(o)).ToList();

                    var rowsOrdersAffected = await connection.ExecuteAsync(orderSql, insertedOrderValues);
                    if (rowsOrdersAffected == 0)
                    {
                        throw new Exception("No dummy orders were seeded into the database.");
                    }
                    logger.LogInformation("Seeded {RowsOrdersAffected} orders into the database.", rowsOrdersAffected);
                }

                // Commit the transaction if everything is successful  
                transaction.Commit();
                logger.LogInformation("Finished seeding");
            }
            catch (Exception ex)
            {
                // Rollback the transaction in case of an error  
                await transaction.RollbackAsync();
                logger.LogError("Transaction rolled back due to an error: {ErrorMessage}", ex.Message);
            }
        }
        
    }
}
