using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FizeRegistration.Domain.Entities.Identity;

public class Agencion
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
    
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public long? UserIdentityId { get; set; }
    public string Color { get; set; }
    public string AgencyName { get; set; }
    public string WebSite { get; set; }
    public string LinkLogo { get; set; } //Picture
    public string LinkPictureUser { get; set; } //Picture
    public string LastName { get; set; }
    public string Link { get; set; }
    public string FirstName { get; set; }
    public string PhoneNumber { get; set; }
}