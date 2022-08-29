using Microsoft.AspNetCore.Components.Forms;

namespace FizeRegistration.Shared.DataContracts;

public sealed class AgencyDataContract
{
    public string UserIdentityId { get; set; }
    public string Color { get; set; }
    public string AgencyName { get; set; }
    public string WebSite { get; set; }
    public string LastName { get; set; }
    public string Link { get; set; }
    public string LinkLogo { get; set; }
    public string LinkPicture { get; set; }
    public string FirstName { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
}
public sealed class NewFileContract
{
   public MultipartFormDataContent Photo { get; set; }
    public string Email { get; set; }
}