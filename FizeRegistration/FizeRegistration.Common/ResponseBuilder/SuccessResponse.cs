﻿using System.Net;
using FizeRegistration.Common.ResponseBuilder.Contracts;


namespace FizeRegistration.Common.ResponseBuilder;
public class SuccessResponse : IWebResponse
{
    public object Body { get; set; }

    public string Message { get; set; }

    public HttpStatusCode StatusCode { get; set; }
}
