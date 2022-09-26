using Microsoft.AspNetCore.Components.Forms;

namespace FizeRegistration.Shared.DataContracts;

public sealed class AgencyDataContract
{
    public AgencyDataContract()
    {

    }
    public AgencyDataContract(AgencyDataContract agencion)
    {
        Id = agencion.Id;
        UserIdentityId = agencion.UserIdentityId;
        Color = agencion.Color;
        AgencyName = agencion.AgencyName;
        WebSite = agencion.WebSite;
        LinkLogo = agencion.LinkLogo;
        LinkPictureUser = agencion.LinkPictureUser;
        LastName = agencion.LastName;
        Link = agencion.Link;
        FirstName = agencion.FirstName;
        PhoneNumber = agencion.PhoneNumber;
    }
    public string Id { get; set; }
    public string UserIdentityId { get; set; }
    public string Color { get; set; }
    public string AgencyName { get; set; }
    public string WebSite { get; set; }
    public string LastName { get; set; }
    public string Link { get; set; }
    public string LinkLogo { get; set; }
    public string LinkPictureUser { get; set; }
    public string FirstName { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }

    public bool IsDelete { get; set; }

    public string BackgroundColor { get; set; }
}
public sealed class NewFileContract
{
   public MultipartFormDataContent Photo { get; set; }
    public string Email { get; set; }
}