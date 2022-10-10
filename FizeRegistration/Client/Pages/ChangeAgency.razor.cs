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
    public partial class ChangeAgency
    {
        [Parameter] public string Id { get; set; }
        public string Email { get; set; }

        private AgencyInformation _agencyInformation = new AgencyInformation();
        private AgencyDataContract? _agencyDataContracts { get; set; }
        [Inject] NavigationManager navigate { get; set; }
        [Inject] IFizeHttpService HttpClient { get; set; }
        private string _urlPicture;
        private string _urlLogo;

        [Inject] AuthenticationStateProvider AuthStateProvider { get; set; }

        private bool isLoading;
        private List<IBrowserFile> loadedPicture = new();
        private List<IBrowserFile> loadedLogo = new();

        private int maxAllowedFiles = 3;

        private string extensionname = "default";

        private string base64PictureData = "";
        private bool SuccessfulAgency;

        protected async override Task OnInitializedAsync()
        {
            if (Id != null)
            {
                var formDataLogo = new MultipartFormDataContent();
                var fileLogo = new StringContent(Id);
                formDataLogo.Add(fileLogo, "agencyId");
                var sendConfirmationResponse = await HttpClient.GetAgencyById(formDataLogo);
                var stringAgency = sendConfirmationResponse.Message;
                 _agencyDataContracts = JsonConvert.DeserializeObject<AgencyDataContract>(stringAgency);
                _agencyInformation.WebSite = _agencyDataContracts.WebSite;
                _agencyInformation.Link = _agencyDataContracts.Link;
                _agencyInformation.PhoneNumbers = _agencyDataContracts.PhoneNumber;
                _agencyInformation.LastName = _agencyDataContracts.LastName;
                _agencyInformation.FirstName = _agencyDataContracts.FirstName;
                _agencyInformation.AgencyName = _agencyDataContracts.AgencyName;
                _agencyInformation.Color = _agencyDataContracts.Color;
                _urlPicture = _agencyDataContracts.LinkPictureUser;
                _urlLogo = _agencyDataContracts.LinkLogo;
            }
            var user = (await AuthStateProvider.GetAuthenticationStateAsync()).User;

            var a = user.Claims.First(c => c.Type == ClaimTypes.Email);

            if (string.IsNullOrEmpty(a.ToString()))
            {
                await HttpClient.SetTokenToLocalStorageAndHeader(new TokenDataContract());
                await AuthStateProvider.GetAuthenticationStateAsync();
            }

            Email = a.ToString();
        }
        public async Task CheckAgency()
        {
            navigate.NavigateTo("/app/agency/all");

        }

        public async Task ContinueNext()
        {
            var formDataLogo = new MultipartFormDataContent();
            var output = new MemoryStream();

            if (_agencyInformation.Logo != null)
            {
                StreamContent fileLogo = new StreamContent(_agencyInformation.Logo.OpenReadStream());
                formDataLogo.Add(fileLogo, "fileLogo", Guid.NewGuid().ToString());
            }
            if (_agencyInformation.Picture != null)
            {
                StreamContent filePictire = new StreamContent(_agencyInformation.Picture.OpenReadStream());
                formDataLogo.Add(filePictire, "filePictire", Guid.NewGuid().ToString());
                await _agencyInformation.Picture.OpenReadStream().CopyToAsync(output);

            }
            AgencyDataContract agencyDataContract = new AgencyDataContract
            {
                Id = _agencyDataContracts.Id,
                AgencyName = _agencyInformation.AgencyName,
                FirstName = _agencyInformation.FirstName,
                Color = _agencyInformation.Color,
                LastName = _agencyInformation.LastName,
                Link = _agencyInformation.Link,
                PhoneNumber = _agencyInformation.PhoneNumbers,
                WebSite = _agencyInformation.WebSite,
                Email = Email,
                LinkLogo = _agencyDataContracts.LinkLogo,
                LinkPictureUser = _agencyDataContracts.LinkPictureUser
            };

            var jsonAgency = JsonConvert.SerializeObject(agencyDataContract);
            var stringContentAgency = new StringContent(jsonAgency);
            formDataLogo.Add(stringContentAgency, "agencyData");

            var sendConfirmationResponse = await HttpClient.ChangeAgency(formDataLogo);
            int statusCode = (int)sendConfirmationResponse.StatusCode;

            if (statusCode >= 200 && statusCode < 300)
            {
                SuccessfulAgency = true;
            }
        }

        private async void OnLinkLogoFilesChange(InputFileChangeEventArgs e)
        {
            _agencyInformation.Logo = e.File;
        }

        private async Task OnLinkPictureFilesChange(InputFileChangeEventArgs e)
        {
            loadedPicture.Clear();
            foreach (var file in e.GetMultipleFiles(maxAllowedFiles))
            {
                try
                {
                    loadedPicture.Add(file);


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
                        base64PictureData = "data:image/png;base64," + Convert.ToBase64String(buf);

                        _agencyInformation.Picture = e.File;
                        isLoading = true;
                    }
                    else
                    {

                    };
                }
                catch (Exception ex)
                {
                }
            }
        }
    }
}
