namespace FizeRegistration.Shared.DataContracts;

public sealed class NewDetailsDataContract
{
    public string UserIdentityId { get; set; }
    public string Color { get; set; }
    public string AgencyName { get; set; }
    public string WebSite { get; set; }
    public string LinkLogo { get; set; } //Picture
    public string LinkPictureUser { get; set; } //Picture
    public string LastName { get; set; }
    public string Link { get; set; }
    public string FirstName { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
}
