using System.Net;

namespace FizeRegistration.Shared.ResponseContracts;

public interface IWebResponse
{
    object Body { get; set; }

    string Message { get; set; }

    HttpStatusCode StatusCode { get; set; }
}
