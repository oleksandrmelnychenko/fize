using FizeRegistration.Client.Services;
using FizeRegistration.Client.Services.HttpService.Contracts;
using FizeRegistration.Shared.DataContracts;
using FizeRegistration.Shared.DataEmail;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Newtonsoft.Json;
using System.Security.Claims;

namespace FizeRegistration.Client.Pages
{

    public partial class DetaliRegistration
    {
        public string Email { get; set; }
        AgencyInformation AgencyInformation = new AgencyInformation();
        [Inject] NavigationManager NavigateManager { get; set; }
        [Inject] IFizeHttpService HttpClient { get; set; }
        [Inject] AuthenticationStateProvider AuthStateProvider { get; set; }

        private bool isLoading;
        private List<IBrowserFile> loadedFiles = new();
        private int maxAllowedFiles = 3;

        private string extensionname = "default";
        private string base64data = "";
        private bool SuccessfulAgency;

        protected async override Task OnInitializedAsync()
        {
            var user = (await AuthStateProvider.GetAuthenticationStateAsync()).User;

            var a = user.Claims.First(c => c.Type == ClaimTypes.Email);

            if (string.IsNullOrEmpty(a.ToString()))
            {
                await HttpClient.SetTokenToLocalStorageAndHeader(new TokenDataContract());
                await AuthStateProvider.GetAuthenticationStateAsync();
            }

            Email = a.Value.ToString();
        }

        public async Task CheckAgency()
        {
                NavigateManager.NavigateTo("/app/agency/all");
        }

        public async Task ContinueNext()
        {
            StreamContent fileLogo = new StreamContent(AgencyInformation.Logo.OpenReadStream());
            StreamContent filePictire = new StreamContent(AgencyInformation.Picture.OpenReadStream());
            var formDataLogo = new MultipartFormDataContent();

            formDataLogo.Add(fileLogo, "fileLogo", Guid.NewGuid().ToString());

            formDataLogo.Add(filePictire, "filePictire", Guid.NewGuid().ToString());

            var output = new MemoryStream();
            await AgencyInformation.Picture.OpenReadStream().CopyToAsync(output);

            AgencyDataContract AgencyDataContract = new AgencyDataContract
            {
                AgencyName = AgencyInformation.AgencyName,
                FirstName = AgencyInformation.FirstName,
                Color = AgencyInformation.Color,
                LastName = AgencyInformation.LastName,
                Link = AgencyInformation.Link,
                PhoneNumber = AgencyInformation.PhoneNumbers,
                WebSite = AgencyInformation.WebSite,
                Email = Email,
            };

            var jsonAgency = JsonConvert.SerializeObject(AgencyDataContract);
            var stringContentAgency = new StringContent(jsonAgency);
            formDataLogo.Add(stringContentAgency, "agencyData");

            var sendConfirmationResponse = await HttpClient.SendFile(formDataLogo);
            int statusCode = (int)sendConfirmationResponse.StatusCode;

            if (statusCode >= 200 && statusCode < 300)
            {
                SuccessfulAgency = true;
            }
        }

        private async void OnLinkLogoFilesChange(InputFileChangeEventArgs e)
        {
            AgencyInformation.Logo = e.File;
        }

        private async Task OnLinkPictureFilesChange(InputFileChangeEventArgs e)
        {
            loadedFiles.Clear();
            foreach (var file in e.GetMultipleFiles(maxAllowedFiles))
            {
                try
                {
                    loadedFiles.Add(file);
                    extensionname = Path.GetExtension(file.Name);

                    var imagefiletypes = new List<string>() {
                    ".png",".jpg",".jpeg"
                };
                    if (imagefiletypes.Contains(extensionname))
                    {
                        var resizedFile = await file.RequestImageFileAsync(file.ContentType, 640, 480);
                        var buf = new byte[resizedFile.Size];
                        using (var stream = resizedFile.OpenReadStream())
                        {
                            await stream.ReadAsync(buf);
                        }
                        base64data = "data:image/png;base64," + Convert.ToBase64String(buf);

                        AgencyInformation.Picture = e.File;
                        isLoading = true;
                    }
                }
                catch (Exception ex)
                {
                }
            }
        }
    }
}
