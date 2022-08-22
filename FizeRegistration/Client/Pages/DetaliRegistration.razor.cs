using FizeRegistration.Client.Helpers.Validation;
using FizeRegistration.Client.Services.HttpService.Contracts;
using FizeRegistration.Shared.DataContracts;
using Microsoft.AspNetCore.Components;

namespace FizeRegistration.Client.Pages
{
    public class MockDetails
    {
       public NewDetailsDataContract newDetailsDataContract = new NewDetailsDataContract
        {

            AgencyName = "biba",
            FirstName = "bibovich",
            LastName = "boba",
            PhoneNumber = "0671306621",
            Link = "xz",
            Color = "nigga",
            Email = "balis77",
            LinkLogo = "linklogo",
            LinkPictureUser = "linkUserPic",
            WebSite = "google",
            
             
        };
    }

    public partial class DetaliRegistration
    {
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

            NewDetailsDataContract newDetailsDataContract = new NewDetailsDataContract
            {
                AgencyName = AgencyName,
                FirstName = FirstName,
                Color = Color,
                LastName = LastName,
                Link = Link,
                PhoneNumber = PhoneNumbers,
                WebSite = WebSite,
            };
            MockDetails mock = new MockDetails();
            await HttpClient.SendDetails(mock.newDetailsDataContract);
        }

    }
}
