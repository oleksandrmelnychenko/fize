using FizeRegistration.Domain.Entities.Identity;

namespace FizeRegistration.Domain.Repositories.Identity.Contracts;

public interface IDetailsRepository
{
    long NewDetails(Agencion details);

    void UpdateDetailsId(long AgencionId, long UserIdentitiesId);

}