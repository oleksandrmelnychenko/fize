using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using FizeRegistration.Common.WebApi;
using FizeRegistration.Common.WebApi.RoutingConfiguration;
using FizeRegistration.Shared.ResponseBuilder.Contracts;
using FizeRegistration.Domain.Entities.Identity;
using FizeRegistration.Common.Exceptions.IdentityExceptions;
using FizeRegistration.Services.IdentityServices.Contracts;
using System.Security.Claims;
using FizeRegistration.Shared.DataContracts;
using FizeRegistration.Common.Helpers;
using Newtonsoft.Json;

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

    [HttpPost]
    [Authorize(Roles = ("UnconfirmedUser"))]
    [AssignActionRoute(IdentitySegments.NEW_ACCOUNT)]
    public async Task<IActionResult> NewUser([FromBody] NewUserDataContract newUserDataContract)
    {
        try
        {
            if (newUserDataContract == null) throw new ArgumentNullException("NewUserDataContract");

            var identity = HttpContext.User.Identity as ClaimsIdentity;

            var emailFromClaims = identity?.FindFirst(ClaimTypes.Email)?.Value;

            if (emailFromClaims == null) throw new ArgumentNullException(nameof(emailFromClaims));

            newUserDataContract.Email = emailFromClaims;

            newUserDataContract.PasswordExpiresAt = DateTime.Now.AddDays(365);

            UserAccount user = await _userIdentityService.NewUser(newUserDataContract);

            return Ok(SuccessResponseBody(user));
        }
        catch (InvalidIdentityException exc)
        {
            return BadRequest(ErrorResponseBody(exc.GetUserMessageException, HttpStatusCode.BadRequest, exc.Body));
        }
        catch (Exception exc)
        {
            return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
        }
    }


    [HttpPost]
    [AllowAnonymous]
    [AssignActionRoute(IdentitySegments.ISSUE_CONFIRMATION)]
    public async Task<IActionResult> IssueConfirmation([FromBody] UserEmailDataContract userEmailDataContract)
    {
        try
        {
            if (userEmailDataContract == null) throw new ArgumentNullException("userEmailDataContract");

            var baseUrl = $"{this.Request.Scheme}://{this.Request.Host}/";

            await _userIdentityService.IssueConfirmation(userEmailDataContract, baseUrl);

            return Ok(SuccessResponseBody(new { Message = "Email was sent" }));
        }
        catch (InvalidIdentityException exc)
        {
            return BadRequest(ErrorResponseBody(exc.GetUserMessageException, HttpStatusCode.BadRequest, exc.Body));
        }
        catch (Exception exc)
        {
            return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
        }
    } 


    [HttpPost]
    [AllowAnonymous]
    [AssignActionRoute(IdentitySegments.NEW_FILES)]
    public async Task<IActionResult> NewFiles( [FromForm] IFormFile fileLogo, [FromForm] IFormFile filePictire, [FromForm] string DetailsData)
    {
        try
        {

            if (filePictire != null && fileLogo != null && DetailsData != null)
            {
                var newDetailsDataContract = JsonConvert.DeserializeObject<NewDetailsDataContract>(DetailsData);
                string exention = ".png";
                string pathPictire = Path.Combine(NoltFolderManager.GetImageFilesFolderPath(), filePictire.FileName + exention);
                string pathLogo = Path.Combine(NoltFolderManager.GetImageFilesFolderPath(), fileLogo.FileName + exention);
                newDetailsDataContract.LinkPicture = pathPictire;
                newDetailsDataContract.LinkLogo = pathLogo;
                await _userIdentityService.NewDetails(newDetailsDataContract);

                using (var stream = new FileStream(pathPictire, FileMode.Create))
                {
                    await filePictire.CopyToAsync(stream);
                } 
                using (var stream = new FileStream(pathLogo, FileMode.Create))
                {
                    await fileLogo.CopyToAsync(stream);
                }
            }
            return Ok(SuccessResponseBody(new { Message = "Files Send"}));
        }
        catch (InvalidIdentityException exc)
        {
            return BadRequest(ErrorResponseBody(exc.GetUserMessageException, HttpStatusCode.BadRequest, exc.Body));
        }
        catch (Exception exc)
        {
            return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
        }
    }
    [HttpPost]
    [AllowAnonymous]
    [AssignActionRoute(IdentitySegments.NEW_DETAILS)]
    public async Task<IActionResult> NewDetails([FromBody] NewDetailsDataContract newDetailsDataContract)
    {
        try
        {
           if (newDetailsDataContract == null) throw new ArgumentNullException("newDetailsDataContract");

            var baseUrl = $"{this.Request.Scheme}://{this.Request.Host}/";

            await _userIdentityService.NewDetails(newDetailsDataContract);
            return Ok(SuccessResponseBody(new { Message = "Details create" }));
        }
        catch (InvalidIdentityException exc)
        {
            return BadRequest(ErrorResponseBody(exc.GetUserMessageException, HttpStatusCode.BadRequest, exc.Body));
        }
        catch (Exception exc)
        {
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
            return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
        }
    }
}
