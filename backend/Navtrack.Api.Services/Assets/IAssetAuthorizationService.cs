using System.Threading.Tasks;
using Navtrack.DataAccess.Model.Assets;
using Navtrack.DataAccess.Model.Users;

namespace Navtrack.Api.Services.Assets;

public interface IAssetAuthorizationService
{
    Task<bool> CurrentUserHasRole(UserDocument userDocument, AssetRoleType assetRoleType, string assetId);
}