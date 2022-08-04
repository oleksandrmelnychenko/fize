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

    private bool BadRequestEmail;
    private bool EmailEmty;

    private string SendMessageBadMail;
    private const string PLEASE_ENTER_CORRECT_MAIL = "please enter correct email";
    private void SignInAsync()
    {
        LogIn = true;
    }

    private async Task SendEmailPost()
    {

        if (Email == String.Empty)
        {
            EmailEmty = true;
            SendMessageBadMail = $"Empty mail ,{PLEASE_ENTER_CORRECT_MAIL}";
            return;
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

            BadRequestEmail = true;
            SendMessageBadMail = $"incorrect input,{PLEASE_ENTER_CORRECT_MAIL}";
           
        }

    }
}