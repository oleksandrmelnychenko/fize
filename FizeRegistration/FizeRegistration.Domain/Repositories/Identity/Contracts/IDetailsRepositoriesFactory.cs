using System.Data;

namespace FizeRegistration.Domain.Repositories.Identity.Contracts;

public interface IAgencyRepositoriesFactory
{
    IAgencyRepository NewAgencyRepository(IDbConnection connection);
}