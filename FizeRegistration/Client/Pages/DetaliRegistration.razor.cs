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
        public string Color { get; set; }
        public string AgencyName { get; set; }
        public string WebSite { get; set; }
        public string LastName { get; set; }
        public string Link { get; set; }
        [Required]
        [StringLength(15, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 6)]
        public string FirstName { get; set; }
        public string PhoneNumbers { get; set; }
        public IBrowserFile Logo { get; set; }
        public IBrowserFile Picture { get; set; }
    }
    public partial class DetaliRegistration
    {
        DetailsInformation detailsInformation = new DetailsInformation();
         //private string Color { get; set; }
         //private string AgencyName { get; set; }
         //private string WebSite { get; set; }
         //private string LastName { get; set; }
         //private string Link { get; set; }
         //[Required]
         //[StringLength(10, ErrorMessage = "Name is too long.")]
         //private string FirstName { get; set; }
         //private string PhoneNumbers { get; set; }
         //private IBrowserFile Logo { get; set; }
         //private IBrowserFile Picture { get; set; }

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
         
            isPhoneNumber = PhoneNumber.IsPhoneNbr(detailsInformation.PhoneNumbers);
            var formDataLogo = new MultipartFormDataContent();

            var fileLogo = new StreamContent(detailsInformation.Logo.OpenReadStream());
            var filePictire = new StreamContent(detailsInformation.Picture.OpenReadStream());
            formDataLogo.Add(fileLogo, "fileLogo", Guid.NewGuid().ToString());
            formDataLogo.Add(filePictire, "filePictire", Guid.NewGuid().ToString());

            isPhoneNumber = PhoneNumber.IsPhoneNbr(detailsInformation.PhoneNumbers);
            
            var output = new MemoryStream();
            await detailsInformation.Picture.OpenReadStream().CopyToAsync(output);

        
            NewDetailsDataContract MockDetailsDataContract = new NewDetailsDataContract
            {

                AgencyName = "biba",
                FirstName = "bibovich",
                LastName = "boba",
                PhoneNumber = "0671306621",
                Link = "xz",
                Color = "nigga",
                Email = "balis77",
                WebSite = "google",
            };

            var st = JsonConvert.SerializeObject(MockDetailsDataContract);
            
            var str = new StringContent(st);
            formDataLogo.Add(str, "DetailsData");
            
            //NewDetailsDataContract newDetailsDataContract = new NewDetailsDataContract
            //{
            //    AgencyName = AgencyName,
            //    FirstName = FirstName,
            //    Color = Color,
            //    LastName = LastName,
            //    LinkLogo = LinkLogo,
            //    LinkPictureUser = LinkPicture,
            //    Link = Link,
            //    PhoneNumber = PhoneNumbers,
            //    WebSite = WebSite,
            //};
            await HttpClient.SendFile(formDataLogo);

        }
    }
  
}
