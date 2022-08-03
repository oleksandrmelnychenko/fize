

namespace FizeRegistration.Shared.ResponseBuilder.Contracts;

public interface IResponseFactory
{
    IWebResponse GetSuccessReponse();

    IWebResponse GetErrorResponse();
}