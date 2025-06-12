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

    public Task<CustomerDto> Handle(GetCustomerDetailsQuery request, CancellationToken cancellationToken)
    {
        const string sql = "SELECT " +
                               "[Customer].[Id], " +
                               "[Customer].[Name], " +
                               "[Customer].[Email], " +
                               "[Customer].[WelcomeEmailWasSent] " +
                               "FROM orders.v_Customers AS [Customer] " +
                               "WHERE [Customer].[Id] = @CustomerId ";

        var connection = _sqlConnectionFactory.GetOpenConnection();

        return connection.QuerySingleAsync<CustomerDetailsDto>(sql, new
        {
            request.CustomerId
        });
    }
}
