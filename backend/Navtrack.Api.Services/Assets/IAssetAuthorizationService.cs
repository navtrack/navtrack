using System.Threading.Tasks;
using Navtrack.DataAccess.Model.Assets;

namespace Navtrack.Api.Services.Assets;

public interface IAssetAuthorizationService
{
    Task<bool> CurrentUserHasRole(AssetRoleType assetRoleType, string assetId);
}