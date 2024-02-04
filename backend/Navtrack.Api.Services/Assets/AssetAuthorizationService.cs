using System.Threading.Tasks;
using Navtrack.Api.Services.User;
using Navtrack.DataAccess.Model.Assets;
using Navtrack.DataAccess.Model.Users;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Api.Services.Assets;

[Service(typeof(IAssetAuthorizationService))]
public class AssetAuthorizationService(ICurrentUserAccessor userAccessor) : IAssetAuthorizationService
{
    public async Task<bool> CurrentUserHasRole(string assetId, AssetRoleType assetRoleType)
    {
        UserDocument currentUser = await userAccessor.Get();

        return AssetAuthorize.UserHasAssetRole(currentUser, assetId, assetRoleType);
    }
}