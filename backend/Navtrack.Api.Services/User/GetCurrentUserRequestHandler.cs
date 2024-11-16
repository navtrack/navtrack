using System.Threading.Tasks;
using Navtrack.Api.Model.User;
using Navtrack.Api.Services.Common.Context;
using Navtrack.Api.Services.Requests;
using Navtrack.Api.Services.User.Mappers;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Api.Services.User;

[Service(typeof(IRequestHandler<GetCurrentUserRequest,CurrentUser>))]
public class GetCurrentUserRequestHandler(INavtrackContextAccessor navtrackContextAccessor)
    : BaseRequestHandler<GetCurrentUserRequest, CurrentUser>
{
    public override Task<CurrentUser> Handle(GetCurrentUserRequest request)
    {
        CurrentUser result = CurrentUserMapper.Map(navtrackContextAccessor.NavtrackContext.User);

        return Task.FromResult(result);
    }
}