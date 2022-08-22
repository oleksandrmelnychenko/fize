using System.Data;

namespace FizeRegistration.Domain.Repositories.Identity.Contracts;

public interface IDetailsRepositoriesFactory
{
    IDetailsRepository NewDetailsRepository(IDbConnection connection);
}