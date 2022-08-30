using FizeRegistration.Shared.ResponseBuilder.Contracts;


namespace FizeRegistration.Shared.ResponseBuilder;

public class ResponseFactory : IResponseFactory
{
    public IWebResponse GetSuccessReponse()
    {
        return new SuccessResponse();
    }

    public IWebResponse GetErrorResponse()
    {
        return new ErrorResponse();
    }
}
