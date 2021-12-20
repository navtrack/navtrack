using Navtrack.DataAccess.Model.Assets;

namespace Navtrack.Api.Services.Roles;

public interface IRoleService
{
    void CheckRole(AssetDocument asset, AssetRoleType assetRoleType);
}