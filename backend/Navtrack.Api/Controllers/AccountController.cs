using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Navtrack.Api.Model.Account;
using Navtrack.Api.Model.Common;
using Navtrack.Api.Services.Account;
using Navtrack.Api.Services.Requests;

namespace Navtrack.Api.Controllers;

[ApiController]
public class AccountController(IRequestHandler requestHandler) : ControllerBase
{
    [HttpPost(ApiPaths.Account)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> CreateAccount([FromBody] CreateAccount model)
    {
        await requestHandler.Handle(new CreateAccountRequest
        {
            Model = model
        });

        return Ok();
    }

    [HttpPost(ApiPaths.AccountForgotPassword)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> ForgotPassword([FromBody] ForgotPassword model)
    {
        await requestHandler.Handle(new ForgotPasswordRequest
        {
            Model = model
        });

        return Ok();
    }

    [HttpPost(ApiPaths.AccountResetPassword)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> ResetPassword([FromBody] ResetPassword model)
    {
        await requestHandler.Handle(new ResetPasswordRequest
        {
            Model = model
        });
        
        return Ok();
    }
}