using System;
using System.Collections.Generic;
using System.Text;

namespace FizeRegistration.Shared.DataContracts;

public enum SignInErrorResponseType
{
    InvalidEmail,
    InvalidCredentials,
    PasswordExpired,
    NotAllowed,
    InvalidToken,
    TokenExpired,
    UserDeleted
}
