using FizeRegistration.Client.Helpers.Validation;
using FizeRegistration.Client.Services.HttpService.Contracts;
using FizeRegistration.Shared.DataContracts;
using Microsoft.AspNetCore.Components;

namespace FizeRegistration.Client.Pages
{
    public partial class SignUp : ComponentBase
    {
        [Inject] NavigationManager navigate { get; set; }
        [Inject] IFizeHttpService HttpClient { get; set; }

        private string Email = String.Empty;

        private bool SendMail;

        private bool LogIn;

        private bool BadRequestEmail;
        private bool LoadingProcess;

        private string SendMessageBadMail;
        private const string PLEASE_ENTER_CORRECT_MAIL = "please enter correct email";
        private void SignInAsync()
        {
            LogIn = true;
            navigate.NavigateTo("/auth/signinup");
        }

        private async Task SendEmailPost()
        {
            LoadingProcess = true;
            if (Email == String.Empty || !EmailValidator.IsEmailValid(Email))
            {
                BadRequestEmail = true;
                SendMessageBadMail = $"Empty mail ,{PLEASE_ENTER_CORRECT_MAIL}";
                LoadingProcess = false;
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
            LoadingProcess = false;
        }
    }
}