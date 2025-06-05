using System.Data;

namespace Ordering.Application.Configuration.Data;

public interface ISqlConnectionFactory
{
    IDbConnection CreateOpenConnection();
}
