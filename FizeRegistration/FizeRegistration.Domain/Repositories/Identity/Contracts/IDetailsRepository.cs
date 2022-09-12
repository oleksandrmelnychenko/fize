using FizeRegistration.Domain.Entities.Identity;
using FizeRegistration.Shared.DataContracts;

namespace FizeRegistration.Domain.Repositories.Identity.Contracts;

public interface IAgencyRepository
{
    long AddAgency(Agencion details);

    long ChangeAgency(Agencion details);

    void UpdateAgencyId(long AgencionId, long UserIdentitiesId);

    List<AgencyDataContract> GetAgency();
    AgencyDataContract GetAgencyByID(string Id);

}