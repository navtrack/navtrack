using Navtrack.Api.Model.Assets;
using Navtrack.DataAccess.Model.Assets;

namespace Navtrack.Api.Services.Mappers.Assets;

public static class AssetUserRoleModelMapper
{
    public static AssetUserRoleModel Map(AssetUserRoleElement source)
    {
        return new AssetUserRoleModel
        {
            UserId = source.UserId.ToString(),
            Role = source.Role,
            CreatedDate = source.CreatedDate
        };
    }
}