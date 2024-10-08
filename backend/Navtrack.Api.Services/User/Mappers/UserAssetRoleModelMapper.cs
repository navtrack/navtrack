using Navtrack.Api.Model.User;
using Navtrack.DataAccess.Model.Users;

namespace Navtrack.Api.Services.User.Mappers;

public static class UserAssetRoleModelMapper
{
    public static UserAssetRoleModel Map(UserAssetRoleElement source)
    {
        return new UserAssetRoleModel
        {
            AssetId = source.AssetId.ToString(),
            Role = source.Role
        };
    }
}