using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using FizeRegistration.Domain.Entities.Identity;
using FizeRegistration.Shared.DataContracts;

namespace FizeRegistration.Services.IdentityServices.Contracts;

public interface IUserIdentityService
{

    Task<TokenDataContract> SignInAsync(AuthenticationDataContract authenticateDataContract);


    Task<UserAccount> ValidateToken(ClaimsPrincipal userPrincipal);

    Task<UserAccount> NewUser(NewUserDataContract newUserDataContract);

    Task IssueConfirmation(UserEmailDataContract userEmailDataContract, string baseUrl);

    Task NewAgency(AgencyDataContract agencyDataContract);

    Task ChangeAgency(AgencyDataContract agencyDataContract);

    Task<List<AgencyDataContract>> GetAgency();

    Task<List<AgencyDataContract>> FilterAgency(TableFilterContract filterParametry);

    Task<AgencyDataContract> GetAgencyById(string Id);

    Task<List<AgencyDataContract>> DeleteAgency(string id);

    Task DeleteListAgency(List<AgencyDataContract> listAgency);

}
