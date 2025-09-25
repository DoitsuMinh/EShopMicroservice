using Dapper;
using Npgsql;
using Ordering.Application.Configuration.CQRS.Queries;
using Ordering.Application.Configuration.Data;
using Ordering.Domain.Customers.Exceptions;

namespace Ordering.Application.Customers.GetCustomerDetails;

public class GetCustomerDetailsQueryHandler : IQueryHandler<GetCustomerDetailsQuery, CustomerDetailsDto>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public GetCustomerDetailsQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<CustomerDetailsDto> Handle(GetCustomerDetailsQuery request, CancellationToken cancellationToken)
    {
        var connection = (NpgsqlConnection)_sqlConnectionFactory.GetOpenConnection();

        const string sql = @"
            SELECT 
                c.""Id"", 
                c.""Name"", 
                c.""Email"", 
                c.""WelcomeEmailWasSent"" 
            FROM orders.v_Customers AS c 
            WHERE c.""Id"" = @CustomerId";

        var customer = await connection.QuerySingleOrDefaultAsync<CustomerDetailsDto>(sql, new { request.CustomerId });

        return customer ?? throw new CustomerNotFoundException(request.CustomerId);
    }
}
