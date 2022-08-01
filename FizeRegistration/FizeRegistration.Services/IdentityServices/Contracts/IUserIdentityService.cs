using System.Security.Claims;
using System.Threading.Tasks;
using FizeRegistration.Domain.DataContracts;


public interface IUserIdentityService
{

    Task<UserAccount> SignInAsync(AuthenticationDataContract authenticateDataContract);


    Task<UserAccount> ValidateToken(ClaimsPrincipal userPrincipal);

    Task<UserAccount> NewUser(NewUserDataContract newUserDataContract);
}
