using FizeRegistration.Domain.Entities.Identity;
using FizeRegistration.Shared.DataContracts;

namespace FizeRegistration.Domain.Repositories.Identity.Contracts;

public interface IAgencyRepository
{
    long AddAgency(Agencion details);

    List<AgencyDataContract> FilterAgency(TableFilterContract filterParameter);

    void ChangeAgency(Agencion Agency);

    void UpdateAgencyId(long AgencionId, long UserIdentitiesId);

    List<AgencyDataContract> GetAgency();

    AgencyDataContract GetAgencyByID(string Id);

    List<AgencyDataContract> DeleteAgency(string id);

    void ChangeValueTable(string id,string columnName,string value);
}