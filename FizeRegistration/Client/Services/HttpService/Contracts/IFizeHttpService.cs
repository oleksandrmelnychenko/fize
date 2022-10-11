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

    Task<IWebResponse> ChangeAgency(MultipartFormDataContent model);

    Task<IWebResponse> GetFilterAgency(MultipartFormDataContent model);

    Task<IWebResponse> SendConfirmation(string password);

    Task<IWebResponse> SignInAsync(AuthenticationDataContract authenticationDataContract);

    Task<IWebResponse> GetAgency();

    Task<IWebResponse> GetAgencyById(MultipartFormDataContent id);

    Task<IWebResponse> DeleteAgency(MultipartFormDataContent model);

    Task<IWebResponse> DeleteListAgency(MultipartFormDataContent model);

    Task<IWebResponse> ChangeColumnValue(MultipartFormDataContent model);

    Task<IWebResponse> ChangeFile(MultipartFormDataContent model);
}
