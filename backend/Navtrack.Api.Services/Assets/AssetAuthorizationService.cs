using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using Navtrack.Api.Services.User;
using Navtrack.DataAccess.Model.Assets;
using Navtrack.DataAccess.Model.Users;
using Navtrack.Library.DI;

namespace Navtrack.Api.Services.Assets;

[Service(typeof(IAssetAuthorizationService))]
public class AssetAuthorizationService : IAssetAuthorizationService
{
    private readonly ICurrentUserAccessor currentUserAccessor;

    public AssetAuthorizationService(ICurrentUserAccessor currentUserAccessor)
    {
        this.currentUserAccessor = currentUserAccessor;
    }

    public async Task<bool> CurrentUserHasRole(UserDocument userDocument, AssetRoleType assetRoleType, string assetId)
    {
        UserDocument currentUser = await currentUserAccessor.GetCurrentUser();

        AssetRoleType[] validRoles = assetRoleType == AssetRoleType.Viewer
            ? new[] { AssetRoleType.Owner, AssetRoleType.Viewer }
            : new[] { assetRoleType };

        bool hasRole =
            (currentUser.AssetRoles?.Any(x => x.AssetId == ObjectId.Parse(assetId) && validRoles.Contains(x.Role)))
            .GetValueOrDefault();

        return hasRole;
    }
}