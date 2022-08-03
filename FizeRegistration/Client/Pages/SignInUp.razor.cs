using FizeRegistration.Client.Helpers.Validation;
using FizeRegistration.Client.Services.HttpService.Contracts;
using FizeRegistration.Shared.DataContracts;
using Microsoft.AspNetCore.Components;

namespace FizeRegistration.Client.Pages;

public partial class SignInUp : ComponentBase
{
    [Inject] IFizeHttpService HttpClient { get; set; }

    private string Email = String.Empty;

    private bool SendMail;
    
    private bool LogIn;

    private void SignInAsync()
    {
        LogIn = true;
    }

    private async Task SendEmailPost()
    {
        if (!EmailValidator.IsEmailValid(Email))
        {
            throw new Exception("Email is not valid");
            // need to show an alert etc
        }

        var sendEmailResponse = await HttpClient.SendEmailForSignUp(new UserEmailDataContract
        {
            Email = Email
        });

        int statusCode = (int)sendEmailResponse.StatusCode;

        if (statusCode >= 200 && statusCode < 300)
        {
            SendMail = true;
        }
        else
        {
            var err = System.Text.Json.JsonSerializer.Serialize(sendEmailResponse);

            Console.WriteLine(err);

            throw new Exception("An errorneous response from server");

            // need to show an alert etc
        }

    }
}