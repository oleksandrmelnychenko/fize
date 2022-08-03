using System.Net;


namespace FizeRegistration.Shared.ResponseContracts;

public class SuccessResponse : IWebResponse
{
    public object Body { get; set; }

    public string Message { get; set; }

    public HttpStatusCode StatusCode { get; set; }
}
