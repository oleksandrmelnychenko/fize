using FizeRegistration.Client.Helpers.Validation;
using FizeRegistration.Client.Services.HttpService.Contracts;
using FizeRegistration.Shared.DataContracts;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Text;

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
        [RegularExpression(@"/((([A-Za-z]{3,9}:(?:\/\/)?)(?:[-;:&=\+\$,\w]+@)?[A - Za - z0 - 9.-] +| (?: www.|[-;:&=\+\$,\w]+@)[A-Za-z0-9.-] +)((?:\/[\+~%\/.\w - _] *)?\?? (?:[-\+= &;%@.\w_]*)#?(?:[\w]*))?)/$", ErrorMessage = "Characters are not allowed")]
        public string WebSite { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 6)]
        public string LastName { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 6)]
        public string Link { get; set; }

        [Required]
        [StringLength(15, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 6)]
        public string FirstName { get; set; }

        [Required]
        [RegularExpression(@"^([\+]?33[-]?|[0])?[1-9][0-9]{8}$", ErrorMessage = "Characters are not allowed")]
        public string PhoneNumbers { get; set; }

        public IBrowserFile Logo { get; set; }
        public IBrowserFile Picture { get; set; }
    }

    public partial class DetaliRegistration
    {
        [Parameter] public string Email { get; set; }

        AgencyInformation AgencyInformation = new AgencyInformation();

        [Inject] IFizeHttpService HttpClient { get; set; }
        public async Task DiscardChanges()
        {
            AgencyInformation.AgencyName = null;
            AgencyInformation.FirstName = null;
            AgencyInformation.LastName = null;
            AgencyInformation.PhoneNumbers = null;
            AgencyInformation.Link = null;
            AgencyInformation.Color = null;
            StateHasChanged();
        }
        private bool SuccessfulAgency;

        public async Task ContinueNext()
        {

            StreamContent fileLogo = new StreamContent(AgencyInformation.Logo.OpenReadStream());
            StreamContent filePictire = new StreamContent(AgencyInformation.Picture.OpenReadStream());
            var formDataLogo = new MultipartFormDataContent();

            fileLogo = new StreamContent(AgencyInformation.Logo.OpenReadStream());
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
            formDataLogo.Add(stringContentAgency, "DetailsData");

            var sendConfirmationResponse = await HttpClient.SendFile(formDataLogo);
            int statusCode = (int)sendConfirmationResponse.StatusCode;

            if (statusCode >= 200 && statusCode < 300)
            {
                SuccessfulAgency = true;
            }

        }
        private void OnLinkLogoFilesChange(InputFileChangeEventArgs e)
        {
            AgencyInformation.Logo = e.File;
        }
        private void OnLinkPictureFilesChange(InputFileChangeEventArgs e)
        {
            AgencyInformation.Picture = e.File;
        }
    }

}
