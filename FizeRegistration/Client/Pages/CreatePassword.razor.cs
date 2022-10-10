using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text.RegularExpressions;
using FizeRegistration.Client.Services.HttpService.Contracts;
using FizeRegistration.Shared.DataContracts;
using FizeRegistration.Shared.Entities.Identity;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.WebUtilities;

namespace FizeRegistration.Client.Pages;



public class ConfirmPassword
{
    [Required(ErrorMessage = "This field is required.")]
    [RegularExpression(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$", ErrorMessage = "password does not match")]
    public string NewPassword { get; set; }

    [Required(ErrorMessage = "This field is required.")]
    [Compare(nameof(NewPassword), ErrorMessage = "Passwords don't match.")]
    public string ConfirmPasswords { get; set; }
}
public partial class CreatePassword : ComponentBase
{
    [Inject] NavigationManager NavigationManager { get; set; }

    [Inject] IFizeHttpService HttpClient { get; set; }

    private ConfirmPassword password = new ConfirmPassword();
    [Inject] AuthenticationStateProvider AuthStateProvider { get; set; }

    private string SendMessageBadMail;

    private bool SuccessfulPassword;
    private bool LoadingProcess;
    private bool BadRequestEmail;

  
    public async Task SendConfirmation()
    {
        var sendConfirmationResponse = await HttpClient.SendConfirmation(password.NewPassword);

        int statusCode = (int)sendConfirmationResponse.StatusCode;

        if (statusCode >= 200 && statusCode < 300)
        {
            Console.WriteLine("Signed Up");

            await HttpClient.SetTokenToLocalStorageAndHeader(new TokenDataContract());
            SuccessfulPassword = true;
        }
        else
        {
            var err = System.Text.Json.JsonSerializer.Serialize(sendConfirmationResponse);

            Console.WriteLine(err);
            LoadingProcess = false;

            BadRequestEmail = true;
            SendMessageBadMail = sendConfirmationResponse.Message;
           
        }
        LoadingProcess = false;
    }

    protected override async Task OnInitializedAsync()
    {

        if (QueryHelpers.ParseQuery(NavigationManager.Uri.Split('#')[1]).TryGetValue("access_token", out var _accessToken))
        {
            var AccessToken = new TokenDataContract
            {
                Token = _accessToken
            };

            await HttpClient.SetTokenToLocalStorageAndHeader(AccessToken);

            var user = (await AuthStateProvider.GetAuthenticationStateAsync()).User;

            var isAuthenticated = user?.Identity?.IsAuthenticated ?? false;

            var isUnconfirmedRoleInClaims = user?.IsInRole("UnconfirmedUser") ?? false;

            if (!isAuthenticated || !isUnconfirmedRoleInClaims)
            {
                NavigateToPageWithError();
            }
        }
        else
        {
            NavigateToPageWithError();
        }

        void NavigateToPageWithError()
        {
            /* need to add page with error */
            NavigationManager.NavigateTo("/", true);
        }
    }
}
