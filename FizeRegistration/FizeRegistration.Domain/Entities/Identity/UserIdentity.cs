using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FizeRegistration.Domain.Entities.Identity;

public class UserIdentity : EntityBase
{
    public UserIdentity()
    {

    }

    [DataType(DataType.EmailAddress)]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string PasswordHash { get; set; }

    [Required]
    public string PasswordSalt { get; set; }

    public DateTime? LastLoggedIn { get; set; }

    public bool IsPasswordExpired { get; set; } = false;

    public bool ForceChangePassword { get; set; }

    public bool CanUserResetExpiredPassword { get; set; } = true;

    public DateTime PasswordExpiresAt { get; set; } = DateTime.UtcNow.AddYears(1);
    public Details Details { get; set; }
}
public class Details
{
    public string Id { get; set; }
    public string UserIdentityId { get; set; }
    public string Color { get; set; }
    public string AgencyName { get; set; }
    public string WebSite { get; set; }
    public string Logo { get; set; } //Picture
    public string PictureUser { get; set; } //Picture
    public string LastName { get; set; }
    public string Link { get; set; }
    public string FirstName { get; set; }
    public string PhoneNumber { get; set; }
}