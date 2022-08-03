using FizeRegistration.Shared.DataContracts;
using FizeRegistration.Shared.ResponseBuilder.Contracts;

namespace FizeRegistration.Client.Services.HttpService.Contracts;

public interface IFizeHttpService
{
    public Uri? GetBaseAddress();

    Task SetHeader();

    Task<IWebResponse> SendEmailForSignUp(UserEmailDataContract userEmail);
}
