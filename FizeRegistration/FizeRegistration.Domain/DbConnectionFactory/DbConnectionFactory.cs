using System.Data;
using System.Data.SqlClient;
using FizeRegistration.Common;
using FizeRegistration.Domain.DbConnectionFactory.Contracts;

namespace FizeRegistration.Domain.DbConnectionFactory;

public sealed class DbConnectionFactory : IDbConnectionFactory
{
    public IDbConnection NewSqlConnection()
    {
        return new SqlConnection(ConfigurationManager.DatabaseConnectionString);
    }
}
