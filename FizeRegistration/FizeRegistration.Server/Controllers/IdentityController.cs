using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System;
using System.Net;
using System.Threading.Tasks;
using FizeRegistration.Common.WebApi;
using FizeRegistration.Common.WebApi.RoutingConfiguration;
using FizeRegistration.Common.ResponseBuilder.Contracts;

namespace FizeRegistration.Server.Controllers;

[AssignControllerRoute(WebApiEnvironmnet.Current, WebApiVersion.ApiVersion1, ApplicationSegments.UserIdentity)]
public class IdentityController : WebApiControllerBase
{
    private readonly IUserIdentityService _userIdentityService;

    public IdentityController(IUserIdentityService userIdentityService,
         IResponseFactory responseFactory) : base(responseFactory)
    {
        _userIdentityService = userIdentityService;
    }

    [Authorize]
    [HttpGet]
    [AssignActionRoute(IdentitySegments.VALIDATE_TOKEN)]
    public async Task<IActionResult> ValidateToken()
    {
        try
        {
            UserAccount user = await _userIdentityService.ValidateToken(User);
            return Ok(SuccessResponseBody(user));
        }
        catch (InvalidIdentityException exc)
        {
            return Unauthorized(ErrorResponseBody(exc.GetUserMessageException, HttpStatusCode.Unauthorized, exc.Body));
        }
        catch (Exception exc)
        {
            Log.Error(exc.Message);
            return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
        }
    }

    [HttpPost]
    [AllowAnonymous]
    [AssignActionRoute(IdentitySegments.NEW_ACCOUNT)]
    public async Task<IActionResult> NewUser([FromBody] NewUserDataContract newUserDataContract)
    {
        try
        {
            if (newUserDataContract == null) throw new ArgumentNullException("NewUserDataContract");

            UserAccount user = await _userIdentityService.NewUser(newUserDataContract);

            return Ok(SuccessResponseBody(user));
        }
        catch (InvalidIdentityException exc)
        {
            return BadRequest(ErrorResponseBody(exc.GetUserMessageException, HttpStatusCode.BadRequest, exc.Body));
        }
        catch (Exception exc)
        {
            Log.Error(exc.Message);
            return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
        }
    }

    [HttpPost]
    [AllowAnonymous]
    [AssignActionRoute(IdentitySegments.SIGN_IN)]
    public async Task<IActionResult> SignIn([FromBody] AuthenticationDataContract authenticateDataContract)
    {
        try
        {
            if (authenticateDataContract == null) throw new ArgumentNullException("AuthenticationDataContract");

            UserAccount user = await _userIdentityService.SignInAsync(authenticateDataContract);

            return Ok(SuccessResponseBody(user));
        }
        catch (InvalidIdentityException exc)
        {
            return BadRequest(ErrorResponseBody(exc.GetUserMessageException, HttpStatusCode.BadRequest, exc.Body));
        }
        catch (Exception exc)
        {
            Log.Error(exc.Message);
            return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
        }
    }
}
