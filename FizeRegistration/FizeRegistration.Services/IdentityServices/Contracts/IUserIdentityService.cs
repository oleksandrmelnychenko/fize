using System.Security.Claims;
using System.Threading.Tasks;
using FizeRegistration.Domain.DataContracts;
using FizeRegistration.Domain.Entities.Identity;

namespace FizeRegistration.Services.IdentityServices.Contracts;

public interface IUserIdentityService
{

    Task<UserAccount> SignInAsync(AuthenticationDataContract authenticateDataContract);


    Task<UserAccount> ValidateToken(ClaimsPrincipal userPrincipal);

    Task<UserAccount> NewUser(NewUserDataContract newUserDataContract);
}
