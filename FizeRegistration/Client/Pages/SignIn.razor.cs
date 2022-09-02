using FizeRegistration.Client.Helpers.Validation;
using FizeRegistration.Client.Services.HttpService.Contracts;
using FizeRegistration.Shared.DataContracts;
using Microsoft.AspNetCore.Components;

namespace FizeRegistration.Client.Pages
{
    public partial class SignIn : ComponentBase
    {
        [Inject] NavigationManager NavigationManager { get; set; }
        private string SendMessageBadMail;

        //private bool SuccessfulPassword;
        private bool LoadingProcess;
        private bool BadRequestEmail;
        private string Email { get; set; }

        private string Password { get; set; }

        public async Task Login()
        {

            LoadingProcess = true;
            if (String.IsNullOrWhiteSpace(Password))
            {
                Console.WriteLine("Empty password");
                SendMessageBadMail = "Empty password";
                LoadingProcess = false;
                BadRequestEmail = true;
                return;
            }

            var authenticationDataContract = new AuthenticationDataContract
            {
                Email = Email,
                Password = Password
            };

            var signInResponse = await HttpClient.SignInAsync(authenticationDataContract);

            int statusCode = (int)signInResponse.StatusCode;

            if (statusCode >= 200 && statusCode < 300)
            {
                Console.WriteLine("Signed In");
                SendMessageBadMail = "Signed In";
                LoadingProcess = false;
                SendEmail.SetData(Email);
                NavigationManager.NavigateTo($"/app/agency/new");
                // need a change of controller to get token
            }
            else
            {
                var err = System.Text.Json.JsonSerializer.Serialize(signInResponse);

                Console.WriteLine(err);

                throw new Exception("An errorneous response from server");
                SendMessageBadMail = "An errorneous response from server";
                LoadingProcess = false;
                BadRequestEmail = true;

                // need to show an alert etc
            }
        }
    }
}
