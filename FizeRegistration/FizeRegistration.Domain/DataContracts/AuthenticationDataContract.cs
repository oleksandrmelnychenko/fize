﻿using System;
using System.Collections.Generic;
using System.Text;

namespace FizeRegistration.Domain.DataContracts;

public class AuthenticationDataContract
{
    public string Email { get; set; }

    public string Password { get; set; }
}

