using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Navtrack.Api.Services.Requests;

namespace Navtrack.Api.Controllers.Shared;

public abstract class NavtrackControllerBase(IRequestHandler requestHandler) : ControllerBase
{
    protected Task<TResult> Query<TRequest, TResult>(TRequest request)
    {
        return requestHandler.Handle<TRequest, TResult>(request);
    }

    protected async Task<IActionResult> Command<TRequest>(TRequest request)
    {
        await requestHandler.Handle(request);

        return Ok();
    }
}
