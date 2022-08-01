using System.Data;

namespace FizeRegistration.Domain.DbConnectionFactory;

public interface IDbConnectionFactory
{
    IDbConnection NewSqlConnection();
}
