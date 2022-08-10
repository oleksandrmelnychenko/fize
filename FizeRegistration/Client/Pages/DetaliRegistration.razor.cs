namespace FizeRegistration.Client.Pages
{
    public partial class DetaliRegistration
    {
        public async Task DiscardChanges()
        {
            AgencyName = null;
            FirstName = null;
            LastName = null;
            PhoneNumber = null;
            Link = null;
            Color = null;
            StateHasChanged();
        }
        public async Task ContinueNext()
        {

        }

    }
}
