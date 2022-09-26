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
using Microsoft.AspNetCore.Components;

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
    [AssignActionRoute(IdentitySegments.FILTER_AGENCY)]
    public async Task<IActionResult> FiletAgency([FromForm] string tableFilterContract)
    {
        try
        {
            var tableFilter = JsonConvert.DeserializeObject<TableFilterContract>(tableFilterContract);

            var listFlter = await _userIdentityService.FilterAgency(tableFilter);

            return Ok(listFlter);
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
    [AssignActionRoute(IdentitySegments.DELETE_AGENCY)]
    public async Task<IActionResult> DeleteAgency([FromForm] string agencyId)
    {
        try
        {
            var result = await _userIdentityService.DeleteAgency(agencyId);
            return Ok(result);
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
    [AssignActionRoute(IdentitySegments.DELETE_LIST_AGENCY)]
    public async Task<IActionResult> DeleteListAgency([FromForm] string deleteList)
    {
       
        try
        {
            List<AgencyDataContract?> agencyListDataContract = JsonConvert.DeserializeObject<List<AgencyDataContract?>>(deleteList);
            await _userIdentityService.DeleteListAgency(agencyListDataContract);

            return Ok(SuccessResponseBody(new { Message = "Files Delete" }));
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


    [HttpGet]
    [AllowAnonymous]
    [AssignActionRoute(IdentitySegments.GET_AGENCY)]
    public async Task<IActionResult> GetAgency()
    {
       
        try
        {
            var result = await _userIdentityService.GetAgency();
            return Ok(result);
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
    [AssignActionRoute(IdentitySegments.GET_AGENCY_BY_ID)]
    public async Task<IActionResult> GetAgencyById([FromForm] string agencyId)
    {
        
        try
        {
            var result = await _userIdentityService.GetAgencyById(agencyId);
            return Ok(result);
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
    [AssignActionRoute(IdentitySegments.CHANGE_AGENCY)]
    public async Task<IActionResult> ChangeAgency([FromForm] IFormFile fileLogo, [FromForm] IFormFile filePictire, [FromForm] string agencyData)
    {
        try
        {
            AgencyDataContract? agencyDataContract = JsonConvert.DeserializeObject<AgencyDataContract>(agencyData);
            string exention = ".png";

            if (fileLogo != null)
            {
                string pathLogo = Path.Combine(NoltFolderManager.GetFilesFolderPath(), NoltFolderManager.GetStaticImageFolder(), fileLogo.FileName + exention);
                agencyDataContract.LinkLogo = Path.Combine(NoltFolderManager.GetStaticImageFolder(), fileLogo.FileName + exention);

                using (var stream = new FileStream(pathLogo, FileMode.Create))
                {
                    await fileLogo.CopyToAsync(stream);
                }
            }

            if (filePictire != null)
            {
                string pathPictire = Path.Combine(NoltFolderManager.GetFilesFolderPath(), NoltFolderManager.GetStaticImageFolder(), filePictire.FileName + exention);
                agencyDataContract.LinkPictureUser = Path.Combine(NoltFolderManager.GetStaticImageFolder(), filePictire.FileName + exention);

                using (var stream = new FileStream(pathPictire, FileMode.Create))
                {
                    await filePictire.CopyToAsync(stream);
                }
            }
            if (agencyData != null)
            {
                await _userIdentityService.ChangeAgency(agencyDataContract);

            }

            return Ok(SuccessResponseBody(new { Message = "Files Send" }));
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
    public async Task<IActionResult> NewFiles([FromForm] IFormFile fileLogo, [FromForm] IFormFile filePictire, [FromForm] string agencyData)
    {
        try
        {
            if (filePictire != null && fileLogo != null && agencyData != null)
            {
                var agencyDataContract = JsonConvert.DeserializeObject<AgencyDataContract>(agencyData);
                string exention = ".png";
                string pathPictire = Path.Combine(NoltFolderManager.GetFilesFolderPath(), NoltFolderManager.GetStaticImageFolder(), filePictire.FileName + exention);
                string pathLogo = Path.Combine(NoltFolderManager.GetFilesFolderPath(), NoltFolderManager.GetStaticImageFolder(), fileLogo.FileName + exention);
                agencyDataContract.LinkPictureUser = Path.Combine(NoltFolderManager.GetStaticImageFolder(), filePictire.FileName + exention);
                agencyDataContract.LinkLogo = Path.Combine(NoltFolderManager.GetStaticImageFolder(), fileLogo.FileName + exention);
                await _userIdentityService.NewAgency(agencyDataContract);
                using (var stream = new FileStream(pathPictire, FileMode.Create))
                {
                    await filePictire.CopyToAsync(stream);
                }
                using (var stream = new FileStream(pathLogo, FileMode.Create))
                {
                    await fileLogo.CopyToAsync(stream);
                }
            }
            return Ok(SuccessResponseBody(new { Message = "Files Send" }));
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

            TokenDataContract tokenDataContract = await _userIdentityService.SignInAsync(authenticateDataContract);

            return Ok(SuccessResponseBody(tokenDataContract));
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
