using System;
using System.Collections.Generic;
using System.Text;

namespace FizeRegistration.Shared.DataContracts;

public sealed class NewUserDataContract
{
    public string Email { get; set; }

    public string Password { get; set; }

    public DateTime PasswordExpiresAt { get; set; }
}
