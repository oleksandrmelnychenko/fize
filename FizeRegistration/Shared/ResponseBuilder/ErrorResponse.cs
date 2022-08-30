using System.Net;
using FizeRegistration.Shared.ResponseBuilder.Contracts;

namespace FizeRegistration.Shared.ResponseBuilder;

public class ErrorResponse : IWebResponse
{
    public object Body { get; set; }

    public string Message { get; set; }

    public HttpStatusCode StatusCode { get; set; }
}