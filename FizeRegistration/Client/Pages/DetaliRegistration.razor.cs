using FizeRegistration.Client.Helpers.Validation;
using FizeRegistration.Client.Services.HttpService.Contracts;
using FizeRegistration.Shared.DataContracts;
using FizeRegistration.Shared.DataEmail;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace FizeRegistration.Client.Pages
{
    public sealed class AgencyInformation
    {
        [Required]
        [StringLength(20, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 6)]
        public string Color { get; set; }
        [Required]
        [StringLength(20, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 6)]
        public string AgencyName { get; set; }

        [Required]
        [RegularExpression(@"^(http|http(s)?://)?([\w-]+\.)+[\w-]+[.com|.in|.org]+(\[\?%&=]*)?", ErrorMessage = "Characters are not allowed")]
        public string WebSite { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 6)]
        public string LastName { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 6)]
        public string Link { get; set; }

        [Required]
        [StringLength(15, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 4)]
        public string FirstName { get; set; }

        [Required]
        [RegularExpression(@"^([\+]?33[-]?|[0])?[1-9][0-9]{8}$", ErrorMessage = "Characters are not allowed")]
        public string PhoneNumbers { get; set; }

        [Required]
        [MaxFileSize(3 * 1024 * 1024)]
        public IBrowserFile Logo { get; set; }
        [Required]
        [MaxFileSize(3 * 1024 * 1024)]
        public IBrowserFile Picture { get; set; }
    }

    public partial class DetaliRegistration
    {
        public string Email { get; set; }

        AgencyInformation AgencyInformation = new AgencyInformation();
        private string _linkImage { get; set; }
        [Inject] NavigationManager navigate { get; set; }
        [Inject] IFizeHttpService HttpClient { get; set; }
        [Inject] ContainEmail SendEmail { get; set; }

        [Inject] AuthenticationStateProvider AuthStateProvider { get; set; }

        private bool isLoading;
        private List<IBrowserFile> loadedFiles = new();
        private int maxAllowedFiles = 3;

        private string extensionname = "default";
        private string base64data = "";
        protected async override Task OnInitializedAsync()
        {
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
                navigate.NavigateTo("/app/agency/table");

        }
        private bool SuccessfulAgency;

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
            //var zalupa = AgencyInformation.Logo.Name;


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
    public static class DataURLServices
    {
        public static string ToDataUrl(this MemoryStream data, string format)
        {
            var span = new Span<byte>(data.GetBuffer()).Slice(0, (int)data.Length);
            return $"data:{format};base64,{Convert.ToBase64String(span)}";
        }

        public static byte[] ToBytes(string url)
        {
            var commaPos = url.IndexOf(',');
            if (commaPos >= 0)
            {
                var base64 = url.Substring(commaPos + 1);
                return Convert.FromBase64String(base64);
            }
            return null;
        }


    }

}
