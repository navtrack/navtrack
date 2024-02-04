using Navtrack.Api.Model.Assets;
using Navtrack.DataAccess.Model.Assets;
using Navtrack.DataAccess.Model.Users;

namespace Navtrack.Api.Services.Mappers.Assets;

public static class AssetUserRoleModelMapper
{
    public static AssetUserRoleModel Map(AssetUserRoleElement source)
    {
        return new AssetUserRoleModel
        {
            UserId = source.UserId.ToString(),
            Role = source.Role
        };
    }
}