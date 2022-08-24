using FizeRegistration.Client.Helpers.Validation;
using FizeRegistration.Client.Services.HttpService.Contracts;
using FizeRegistration.Shared.DataContracts;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;

namespace FizeRegistration.Client.Pages
{
    public partial class DetaliRegistration
    {
        private string Color { get; set; }
        private string AgencyName { get; set; }
        private string WebSite { get; set; }
        private string LastName { get; set; }
        private string Link { get; set; }
        private string FirstName { get; set; }
        private string PhoneNumbers { get; set; }
        private IBrowserFile LinkLogo { get; set; }
        private IBrowserFile LinkPicture { get; set; }

        [Inject] IFizeHttpService HttpClient { get; set; }
        public async Task DiscardChanges()
        {
            AgencyName = null;
            FirstName = null;
            LastName = null;
            PhoneNumbers = null;
            Link = null;
            Color = null;
            StateHasChanged();
        }
        private bool IsPhoneNumber = true;

        public async Task ContinueNext()
        {
            IsPhoneNumber = PhoneNumber.IsPhoneNbr(PhoneNumbers);
            var formDataContent = new MultipartFormDataContent();

            var fileContent = new StreamContent(LinkLogo.OpenReadStream())
            {
            
            };

            formDataContent.Add(fileContent, "file", Guid.NewGuid().ToString());
            //NewDetailsDataContract newsDetailsDataContract = new NewDetailsDataContract
            //{

            //    AgencyName = "biba",
            //    FirstName = "bibovich",
            //    LastName = "boba",
            //    PhoneNumber = "0671306621",
            //    Link = "xz",
            //    Color = "nigga",
            //    Email = "balis77",
            //    LinkLogo = LinkLogo,
            //    LinkPictureUser = LinkPicture,
            //    WebSite = "google",


            //};

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
            await HttpClient.SendFile(formDataContent);
            // await HttpClient.SendDetails(newsDetailsDataContract);
        }

    }
}
