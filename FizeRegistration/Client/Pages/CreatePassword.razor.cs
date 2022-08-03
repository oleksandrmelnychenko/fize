using System.Text.RegularExpressions;
using FizeRegistration.Client.Services.HttpService.Contracts;
using FizeRegistration.Shared.DataContracts;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;

namespace FizeRegistration.Client.Pages;

public partial class CreatePassword : ComponentBase
{
    [Inject] NavigationManager NavigationManager { get; set; }

    [Inject] IFizeHttpService HttpClient { get; set; }

    private string Password;

    private string ConfirmPassword;

    private readonly Regex _regexPattern = new Regex(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$");

    public async Task SendConfirmation()
    {
        bool isPasswordMatch = _regexPattern.IsMatch(Password);

        if (Password != ConfirmPassword || !isPasswordMatch)
        {
            Console.WriteLine("Password != ConfirmPassword || !isPasswordMatch");

            return;
        }

        var sendConfirmationResponse = await HttpClient.SendConfirmation(Password);

        int statusCode = (int)sendConfirmationResponse.StatusCode;

        if (statusCode >= 200 && statusCode < 300)
        {
            Console.WriteLine("Signed Up");
        }
        else
        {
            var err = System.Text.Json.JsonSerializer.Serialize(sendConfirmationResponse);

            Console.WriteLine(err);

            throw new Exception("An errorneous response from server");

            // need to show an alert etc
        }
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