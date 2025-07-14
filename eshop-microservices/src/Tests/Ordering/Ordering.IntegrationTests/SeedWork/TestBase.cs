using Dapper;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Odering.Infrastructure;
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
                           throw new InvalidOperationException(
                               $"Environment variable '{connectionStringEnviromentVariable}' is not set.");

        await using var connection = new NpgsqlConnection(ConnectionString);
        await ClearDatabase(connection);

        ExecutionContext = new ExecutionContextMock();

        ApplicationStartup.Initialize(
            new ServiceCollection(),
            ConnectionString,
            new CachesStore(),
            Logger.None,
            ExecutionContext);
    }

    private async Task ClearDatabase(NpgsqlConnection connection)
    {
        const string sql = "";

        await connection.ExecuteScalarAsync(sql);
    }
}
