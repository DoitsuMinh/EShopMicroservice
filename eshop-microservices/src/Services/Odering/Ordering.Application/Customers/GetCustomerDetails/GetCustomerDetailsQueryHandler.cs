using Npgsql;
using Ordering.Application.Configuration.Data;
using Ordering.Application.Configuration.Queries;

namespace Ordering.Application.Customers.GetCustomerDetails;

public class GetCustomerDetailsQueryHandler : IQueryHandler<GetCustomerDetailsQuery, CustomerDto>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public GetCustomerDetailsQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<CustomerDto> Handle(GetCustomerDetailsQuery request, CancellationToken cancellationToken)
    {
        string sql = "SELECT " +
                               "Customer.\"Id\", " +
                               "Customer.\"Name\", " +
                               "Customer.\"Email\", " +
                               "Customer.\"WelcomeEmailWasSent\" " +
                               "FROM orders.v_Customers AS Customer " +
                               "WHERE Customer.\"Id\" = @CustomerId";

        using var connection = (NpgsqlConnection)_sqlConnectionFactory.GetOpenConnection();

        using var command = new NpgsqlCommand(sql, connection);
        command.Parameters.AddWithValue("@CustomerId", NpgsqlTypes.NpgsqlDbType.Uuid ,request.CustomerId);

        var reader = await command.ExecuteReaderAsync(cancellationToken);
        while (await reader.ReadAsync(cancellationToken))
        {
            var result = new CustomerDto
            {
                Id = !reader.IsDBNull(reader.GetOrdinal("Id")) ? reader.GetGuid(reader.GetOrdinal("Id")) : Guid.Empty,
                Name = !reader.IsDBNull(reader.GetOrdinal("Name")) ? reader.GetString(reader.GetOrdinal("Name")) : string.Empty,
                Email =!reader.IsDBNull(reader.GetOrdinal("Email")) ? reader.GetString(reader.GetOrdinal("Email")) : string.Empty,
                WelcomeEmailWasSent = reader.GetBoolean(reader.GetOrdinal("WelcomeEmailWasSent"))
            };
            return result;
        }

        return null;
    }
}
