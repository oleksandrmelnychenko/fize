using FizeRegistration.Domain.Entities.Identity;

namespace FizeRegistration.Domain.Repositories.Identity.Contracts;

public interface IAgencyRepository
{
    long AddAgency(Agencion details);

    void UpdateAgencyId(long AgencionId, long UserIdentitiesId);

}