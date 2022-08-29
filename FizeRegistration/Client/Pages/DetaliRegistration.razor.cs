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
    public sealed class DetailsInformation
    {
        [Required]
        [StringLength(20, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 6)]
        public string Color { get; set; }
        [Required]
        [StringLength(20, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 6)]
        public string AgencyName { get; set; }

        [Required]
        //[RegularExpression(@"/((([A-Za-z]{3,9}:(?:\/\/)?)(?:[-;:&=\+\$,\w]+@)?[A - Za - z0 - 9.-] +| (?: www.|[-;:&=\+\$,\w]+@)[A-Za-z0-9.-] +)((?:\/[\+~%\/.\w - _] *)?\?? (?:[-\+= &;%@.\w_]*)#?(?:[\w]*))?)/$", ErrorMessage = "Characters are not allowed")]
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
        DetailsInformation detailsInformation = new DetailsInformation();
       
        [Inject] IFizeHttpService HttpClient { get; set; }
        public async Task DiscardChanges()
        {
            detailsInformation.AgencyName = null;
            detailsInformation.FirstName = null;
            detailsInformation.LastName = null;
            detailsInformation.PhoneNumbers = null;
            detailsInformation.Link = null;
            detailsInformation.Color = null;
            StateHasChanged();
        }
        private bool isPhoneNumber = true;
        private bool isPicture;
        private bool isLogo;
        public async Task ContinueNext()
        {

            StreamContent fileLogo = null;
            StreamContent filePictire = null;
            isPhoneNumber = PhoneNumber.IsPhoneNbr(detailsInformation.PhoneNumbers);
            var formDataLogo = new MultipartFormDataContent();

            fileLogo = new StreamContent(detailsInformation.Logo.OpenReadStream());
            formDataLogo.Add(fileLogo, "fileLogo", Guid.NewGuid().ToString());

            filePictire = new StreamContent(detailsInformation.Picture.OpenReadStream());
            formDataLogo.Add(filePictire, "filePictire", Guid.NewGuid().ToString());

            isPhoneNumber = PhoneNumber.IsPhoneNbr(detailsInformation.PhoneNumbers);

            var output = new MemoryStream();
            await detailsInformation.Picture.OpenReadStream().CopyToAsync(output);

            NewDetailsDataContract newDetailsDataContract = new NewDetailsDataContract
            {
                AgencyName = detailsInformation.AgencyName,
                FirstName = detailsInformation.FirstName,
                Color = detailsInformation.Color,
                LastName = detailsInformation.LastName,
                Link = detailsInformation.Link,
                PhoneNumber = detailsInformation.PhoneNumbers,
                WebSite = detailsInformation.WebSite,
            };
            //NewDetailsDataContract MockDetailsDataContract = new NewDetailsDataContract
            //{
            //    AgencyName = "biba",
            //    FirstName = "bibovich",
            //    LastName = "boba",
            //    PhoneNumber = "0671306621",
            //    Link = "xz",
            //    Color = "nigga",
            //    Email = "balis77",
            //    WebSite = "google",
            //};



            var st = JsonConvert.SerializeObject(newDetailsDataContract);
            var str = new StringContent(st);
            formDataLogo.Add(str, "DetailsData");

                await HttpClient.SendFile(formDataLogo);

            

        }
        private void OnLinkLogoFilesChange(InputFileChangeEventArgs e)
        {
            detailsInformation.Logo = e.File;
        }
        private void OnLinkPictureFilesChange(InputFileChangeEventArgs e)
        {
            detailsInformation.Picture = e.File;
        }
    }

}
