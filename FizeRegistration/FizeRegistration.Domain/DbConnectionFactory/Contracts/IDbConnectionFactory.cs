using System.Data;

namespace FizeRegistration.Domain.DbConnectionFactory.Contracts;

public interface IDbConnectionFactory
{
    IDbConnection NewSqlConnection();
}
