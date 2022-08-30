using FizeRegistration.Shared.DataContracts;
using FizeRegistration.Shared.ResponseBuilder.Contracts;

namespace FizeRegistration.Client.Services.HttpService.Contracts;

public interface IFizeHttpService
{
    public Uri? GetBaseAddress();

    Task SetHeader();

    Task SetTokenToLocalStorageAndHeader(TokenDataContract tokenData);

    Task<IWebResponse> SendEmailForSignUp(UserEmailDataContract userEmail);

    Task<IWebResponse> SendFile(MultipartFormDataContent file);

    Task<IWebResponse> SendConfirmation(string password);

    Task<IWebResponse> SignInAsync(AuthenticationDataContract authenticationDataContract);

}
