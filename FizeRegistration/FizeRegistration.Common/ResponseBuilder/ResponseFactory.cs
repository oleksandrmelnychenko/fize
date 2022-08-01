using FizeRegistration.Common.ResponseBuilder.Contracts;


namespace FizeRegistration.Common.ResponseBuilder;

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
