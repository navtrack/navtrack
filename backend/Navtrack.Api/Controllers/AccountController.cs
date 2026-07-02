using System.Threading.Tasks;
using IdentityServer4;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Navtrack.Api.Controllers.Shared;
using Navtrack.Api.Model.Account;
using Navtrack.Api.Services.Account;
using Navtrack.Api.Services.Requests;

namespace Navtrack.Api.Controllers;

[ApiController]
public class AccountController(IRequestHandler requestHandler) : NavtrackControllerBase(requestHandler)
{
    [HttpPost(ApiPaths.Account)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateAccount([FromBody] CreateAccountModel model) =>
        await Command(new CreateAccountRequest
        {
            Model = model
        });

    [HttpDelete(ApiPaths.Account)]
    [Authorize(IdentityServerConstants.LocalApi.PolicyName)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete([FromBody] DeleteAccountModel model) =>
        await Command(new DeleteAccountRequest
        {
            Model = model
        });

    [HttpPost(ApiPaths.AccountForgotPassword)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordModel model) =>
        await Command(new ForgotPasswordRequest
        {
            Model = model
        });

    [HttpPost(ApiPaths.AccountResetPassword)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel model) =>
        await Command(new ResetPasswordRequest
        {
            Model = model
        });
}
