using Microsoft.AspNetCore.Mvc;
using System.Net;
using FizeRegistration.Shared.ResponseBuilder.Contracts;

namespace FizeRegistration.Common.WebApi;

public abstract class WebApiControllerBase : Controller
{
    private readonly IResponseFactory _responseFactory;

    protected WebApiControllerBase(IResponseFactory responseFactory)
    {
        _responseFactory = responseFactory;
    }

    protected IWebResponse SuccessResponseBody(object body, string message = "")
    {
        IWebResponse response = _responseFactory.GetSuccessReponse();
        response.Body = body;
        response.StatusCode = HttpStatusCode.OK;
        response.Message = message;

        return response;
    }

    protected IWebResponse ErrorResponseBody(string message, HttpStatusCode statusCode, object body = null)
    {
        IWebResponse response = _responseFactory.GetErrorResponse();

        response.Message = message;
        response.StatusCode = statusCode;
        response.Body = body;

        return response;
    }
}
