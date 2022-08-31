using System.Text.Json;
using FizeRegistration.Client.Services.HttpService.Contracts;
using FizeRegistration.Shared.DataContracts;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace FizeRegistration.Client.Components.PartOfTheSite
{
    public partial class BlockSignIn
    {
        [Inject] NavigationManager NavigationManager { get; set; }

        [Inject] IFizeHttpService HttpClient { get; set; }

        [Inject] AuthenticationStateProvider AuthState { get; set; }

        private string SendMessageBadMail;

        //private bool SuccessfulPassword;
        private bool LoadingProcess;
        private bool BadRequestEmail;
        private string Email { get; set; }

        private string Password { get; set; }

        public async Task Login()
        {
            try
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

                if (statusCode < 200 && statusCode >= 299)
                {
                    throw new Exception("status code = " + statusCode);
                }

                Console.WriteLine("Signed In");

                SendMessageBadMail = "Signed In";

                LoadingProcess = false;

                var responseBody = signInResponse.Body.ToString() ?? "{}";

                var tokenDataContract = JsonSerializer.Deserialize<TokenDataContract>(responseBody.ToString());

                ArgumentNullException.ThrowIfNull(tokenDataContract, nameof(tokenDataContract));

                await HttpClient.SetTokenToLocalStorageAndHeader(tokenDataContract);

                await AuthState.GetAuthenticationStateAsync();

                NavigationManager.NavigateTo($"/auth/signinup/detail/{Email}");
            }
            catch (Exception err)
            {

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
