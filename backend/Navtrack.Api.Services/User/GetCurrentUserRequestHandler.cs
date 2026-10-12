using System.Threading.Tasks;
using Navtrack.Api.Model.User;
using Navtrack.Api.Services.Common.RequestContext;
using Navtrack.Api.Services.Requests;
using Navtrack.Api.Services.User.Mappers;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Api.Services.User;

[Service(typeof(IRequestHandler<GetCurrentUserRequest,CurrentUserModel>))]
public class GetCurrentUserRequestHandler(INavtrackRequestContextAccessor navtrackRequestContextAccessor)
    : BaseRequestHandler<GetCurrentUserRequest, CurrentUserModel>
{
    public override Task<CurrentUserModel> Handle(GetCurrentUserRequest request)
    {
        CurrentUserModel result = CurrentUserMapper.Map(navtrackRequestContextAccessor.NavtrackContext.CurrentUser);

        return Task.FromResult(result);
    }
}