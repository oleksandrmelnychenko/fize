using System.Text.RegularExpressions;
using FizeRegistration.Client.Services.HttpService.Contracts;
using FizeRegistration.Shared.DataContracts;
using FizeRegistration.Shared.Entities.Identity;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;

namespace FizeRegistration.Client.Pages;

public partial class CreatePasswords : ComponentBase
{
    [Inject] NavigationManager NavigationManager { get; set; }

    [Inject] IFizeHttpService HttpClient { get; set; }

    private string Password;

    private string ConfirmPassword;
    private string SendMessageBadMail;

    private bool SuccessfulPassword;
    private bool LoadingProcess;
    private bool BadRequestEmail;

    // "#?!@$%^&*-" spec symbols This regex will enforce these rules:
    //  • At least one upper case english letter • At least one lower case english letter
    //   • At least one digit • At least one special character • Minimum 8 in length
    private readonly Regex _regexPattern = new Regex(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$");

    public async Task SendConfirmation()
    {

        LoadingProcess = true;
        if (Password == null)
        {
            SendMessageBadMail = "Password Empty";
            LoadingProcess = false;
            BadRequestEmail = true;
            return;
        }
        bool isPasswordMatch = _regexPattern.IsMatch(Password);

        if (Password != ConfirmPassword || !isPasswordMatch)
        {

            SendMessageBadMail = "Password != ConfirmPassword || !isPasswordMatch";
            LoadingProcess = false;
            BadRequestEmail = true;
            return;
        }

        var sendConfirmationResponse = await HttpClient.SendConfirmation(Password);

        int statusCode = (int)sendConfirmationResponse.StatusCode;

        if (statusCode >= 200 && statusCode < 300)
        {
            Console.WriteLine("Signed Up");
            SuccessfulPassword = true;

            // var userAccount = sendConfirmationResponse.Body as UserAccount;

            //if (userAccount == null) ArgumentNullException.ThrowIfNull(userAccount, nameof(userAccount));

            // need a change of controller to get token 
        }
        else
        {
            var err = System.Text.Json.JsonSerializer.Serialize(sendConfirmationResponse);

            Console.WriteLine(err);
            LoadingProcess = false;
            SendMessageBadMail = "An errorneous response from server";
            //throw new Exception("An errorneous response from server");
            // need to show an alert etc
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
        }
    }
}
