using System.Data;

namespace FizeRegistration.Domain.Repositories.Identity.Contracts;

public interface IIdentityRepositoriesFactory
{
    IIdentityRepository NewIdentityRepository(IDbConnection connection);
}
