using Dapper;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Odering.Infrastructure;
using Odering.Infrastructure.Caching;
using Ordering.Application.Configuration.Emails;
using Serilog.Core;

namespace Ordering.IntegrationTests.SeedWork;

public class TestBase
{
    private const string TEMP_CON_STRING = "Server=localhost;Port=5432;Database=OrderTest;User Id=postgres;Password=postgres;Include Error Detail=true";
    protected string ConnectionString;
    protected ExecutionContextMock ExecutionContext;

    [SetUp]
    public async Task BeforeEachTest()
    {
        const string connectionStringEnviromentVariable = "ORDERING_INTEGRATION_TESTS_CONNECTION_STRING";
        ConnectionString = Environment.GetEnvironmentVariable(connectionStringEnviromentVariable) ??
                           TEMP_CON_STRING;

        await using var connection = new NpgsqlConnection(ConnectionString);
        await ClearDatabase(connection);

        ExecutionContext = new ExecutionContextMock();
        //public static IServiceProvider Initialize(
        //  IServiceCollection services,
        //  string connectionString,
        //  ICacheStore cacheStore,
        //  IEmailSender emailSender,
        //  EmailsSettings emailsSetting,
        //  ILogger logger,
        //  IExecutionContextAccessor executionContextAccessor,
        //  bool runQuartz = true)
        ApplicationStartup.Initialize(
            new ServiceCollection(),
            ConnectionString,
            new CacheStore(),
            null,
            null,
            Logger.None,
            ExecutionContext);
    }

    private async Task ClearDatabase(NpgsqlConnection connection)
    {
        const string sql =  "DELETE FROM app.InternalCommands; " +
                            "DELETE FROM app.OutboxMessages; " +
                            "DELETE FROM orders.OrderProducts; " +
                            "DELETE FROM orders.Orders; " +
                            "DELETE FROM payments.Payments; " +
                            "DELETE FROM orders.Customers; ";

        await connection.ExecuteScalarAsync(sql);
    }
}
