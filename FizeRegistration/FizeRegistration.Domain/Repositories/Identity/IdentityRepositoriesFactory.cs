using System.Data;
using FizeRegistration.Domain.Repositories.Identity.Contracts;

namespace FizeRegistration.Domain.Repositories.Identity;

public class IdentityRepositoriesFactory : IIdentityRepositoriesFactory
{
    public IIdentityRepository NewIdentityRepository(IDbConnection connection) =>
        new IdentityRepository(connection);

}
public class DetatailsRepositoryFactory : IDetailsRepositoriesFactory
{
    public IDetailsRepository NewDetailsRepository(IDbConnection connection) =>
        new DetailsRepository(connection);
}