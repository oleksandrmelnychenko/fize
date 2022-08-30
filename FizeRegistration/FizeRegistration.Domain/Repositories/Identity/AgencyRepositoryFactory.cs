using System.Data;
using FizeRegistration.Domain.Repositories.Identity.Contracts;

namespace FizeRegistration.Domain.Repositories.Identity;

public class AgencyRepositoryFactory : IAgencyRepositoriesFactory
{
    public IAgencyRepository NewAgencyRepository(IDbConnection connection) =>
        new AgencyRepository(connection);
}