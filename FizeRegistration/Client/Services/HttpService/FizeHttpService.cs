using System.Net.Http.Headers;
using System.Net.Http.Json;
using Blazored.LocalStorage;
using FizeRegistration.Client.Services.HttpService.Contracts;
using FizeRegistration.Shared.DataContracts;
using FizeRegistration.Shared.ResponseBuilder;
using FizeRegistration.Shared.ResponseBuilder.Contracts;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace FizeRegistration.Client.Services.HttpService;

public sealed class HttpUrls
{
    public const string SEND_EMAIL = "/api/v1/identity/issue/confirmation";
    public const string SEND_CONFIRMATION = "/api/v1/identity/new/account";
    public const string SIGN_IN = "/api/v1/identity/signin";
    public const string SEND_AGENCY = "/api/v1/identity/new/details";
    public const string SEND_AGENCY_FILE = "/api/v1/identity/new/files";
    public const string GET_AGENCY = "/api/v1/identity/get/agency";
    public const string GET_AGENCY_BY_ID = "/api/v1/identity/agency/by/id";
    public const string CHANGE_AGENCY = "/api/v1/identity/change/agency";
    public const string FILTER_AGENCY = "/api/v1/identity/filter/agency";
    public const string DELETE_AGENCY = "/api/v1/identity/delete/agency";
}

public class FizeHttpService : IFizeHttpService
{
    private HttpClient _httpClient;
    private ILocalStorageService _localStorage;
    public FizeHttpService(HttpClient httpClient, ILocalStorageService localStorage)
    {
        _httpClient = httpClient;
        _localStorage = localStorage;
    }

    public Uri? GetBaseAddress()
    {
        return _httpClient?.BaseAddress;
    }
    public async Task<IWebResponse> GetAgency()
    {
        var response = await _httpClient.GetStringAsync(HttpUrls.GET_AGENCY);
       
        return new SuccessResponse
        {
            Body = new Object(),
            Message = response,
            StatusCode = System.Net.HttpStatusCode.Created
        };
    }
    public async Task<IWebResponse> GetAgencyById(MultipartFormDataContent model)
    {
        return await GetRequestAgency(model, HttpUrls.GET_AGENCY_BY_ID);

        //var responce = await _httpClient.PostAsync(HttpUrls.GET_AGENCY_BY_ID, model);

        //var stringresponce = await responce.Content.ReadAsStringAsync();
        //return new SuccessResponse
        //{
        //    Body = new Object(),
        //    Message = stringresponce,
        //    StatusCode = System.Net.HttpStatusCode.Created
        //}; ;
    }
    public async Task<IWebResponse> GetFilterAgency(MultipartFormDataContent model)
    {
        return await GetRequestAgency(model, HttpUrls.FILTER_AGENCY);
        //var responce = await _httpClient.PostAsync(HttpUrls.FILTER_AGENCY, model);
      
        //var stringresponce = await responce.Content.ReadAsStringAsync();
        //return new SuccessResponse
        //{
        //    Body = new Object(),
        //    Message = stringresponce,
        //    StatusCode = System.Net.HttpStatusCode.Created
        //}; ;
    }

    public async Task<IWebResponse> GetRequestAgency(MultipartFormDataContent modelData, string requestUrl)
    {
        var responce = await _httpClient.PostAsync(requestUrl, modelData);
        var stringresponce = await responce.Content.ReadAsStringAsync();
        return new SuccessResponse
        {
            Body = new Object(),
            Message = stringresponce,
            StatusCode = System.Net.HttpStatusCode.Created
        }; ;
    }

    public async Task SetTokenToLocalStorageAndHeader(TokenDataContract tokenData)
    {
        await _localStorage.SetItemAsync<TokenDataContract>("token", tokenData);
        await SetHeader();
    }

    public async Task SetHeader()
    {
        try
        {
            var token = await _localStorage.GetItemAsync<TokenDataContract>("token");

            _httpClient.DefaultRequestHeaders.Authorization = null;

            if (!String.IsNullOrWhiteSpace(token.Token))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token.Token.Replace("\"", ""));
            }
        }
        catch
        {

        }
    }

    public async Task<IWebResponse> SendEmailForSignUp(UserEmailDataContract userEmail)
    {
        return await SendRequest<UserEmailDataContract>(userEmail, HttpUrls.SEND_EMAIL);

    }

    public async Task<IWebResponse> SendConfirmation(string password)
    {
        var newUserDataContract = new NewUserDataContract
        {
            Password = password
        };

        return await SendRequest<NewUserDataContract>(newUserDataContract, HttpUrls.SEND_CONFIRMATION);
    }


    public async Task<IWebResponse> SignInAsync(AuthenticationDataContract authenticationDataContract)
    {
        return await SendRequest<AuthenticationDataContract>(authenticationDataContract, HttpUrls.SIGN_IN);
    }

    private async Task<IWebResponse> SendRequest<T>(T postData, string requestUrl)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync<T>(
                requestUrl, postData);

            if (response.IsSuccessStatusCode)
            {
                var successfulResponse = await response.Content.ReadFromJsonAsync<SuccessResponse>();

                ArgumentNullException.ThrowIfNull(successfulResponse, nameof(successfulResponse));

                return successfulResponse;
            }

            var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();

            ArgumentNullException.ThrowIfNull(errorResponse, nameof(errorResponse));

            return errorResponse;
        }

        catch
        {
            var errorResponse = new ErrorResponse
            {
                Body = new Object(),
                Message = "Autogenerated error",
                StatusCode = System.Net.HttpStatusCode.BadRequest
            };

            return errorResponse;
        }
    }
    public async Task<IWebResponse> SendFile(MultipartFormDataContent model)
    {
        return await SendRequestAgency(model, HttpUrls.SEND_AGENCY_FILE);

        //await _httpClient.PostAsync(HttpUrls.SEND_AGENCY_FILE, model);
        //return new SuccessResponse
        //{
        //    Body = new Object(),
        //    Message = "SuccessResponse",
        //    StatusCode = System.Net.HttpStatusCode.Created
        //}; ;
    }
    public async Task<IWebResponse> ChangeAgency(MultipartFormDataContent model)
    {
       return await SendRequestAgency(model,HttpUrls.CHANGE_AGENCY);
        //await _httpClient.PostAsync(HttpUrls.CHANGE_AGENCY, model);
        //return new SuccessResponse
        //{
        //    Body = new Object(),
        //    Message = "SuccessResponse",
        //    StatusCode = System.Net.HttpStatusCode.Created
        //}; ;
    }

    public async Task<IWebResponse> DeleteAgency(MultipartFormDataContent model)
    {
        await _httpClient.PostAsync(HttpUrls.DELETE_AGENCY,model);
        return new SuccessResponse
        {
            Body = new Object(),
            Message = "SuccessResponse",
            StatusCode = System.Net.HttpStatusCode.Created
        }; ;
    }

    private async Task<IWebResponse> SendRequestAgency(MultipartFormDataContent modelData,string requestUrl)
    {
        await _httpClient.PostAsync(requestUrl, modelData);
        return new SuccessResponse
        {
            Body = new Object(),
            Message = "SuccessResponse",
            StatusCode = System.Net.HttpStatusCode.Created
        }; ;
    }

   


}
