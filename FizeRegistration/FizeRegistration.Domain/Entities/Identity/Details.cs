using Microsoft.AspNetCore.Components.Forms;

namespace FizeRegistration.Domain.Entities.Identity;

public class Details
{
    public string Id { get; set; }
    public string UserIdentityId { get; set; }
    public string Color { get; set; }
    public string AgencyName { get; set; }
    public string WebSite { get; set; }
    public IBrowserFile LinkLogo { get; set; } //Picture
    public IBrowserFile LinkPictureUser { get; set; } //Picture
    public string LastName { get; set; }
    public string Link { get; set; }
    public string FirstName { get; set; }
    public string PhoneNumber { get; set; }
}