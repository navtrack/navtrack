using Navtrack.Api.Model.Assets;
using Navtrack.DataAccess.Model.Assets;
using Navtrack.DataAccess.Model.Users;

namespace Navtrack.Api.Services.Mappers;

public static class AssetUserModelMapper
{
    public static AssetUserModel Map(AssetUserRoleElement assetUserRole, UserDocument user)
    {
        return new AssetUserModel
        {
            UserId = user.Id.ToString(),
            Email = user.Email,
            Role = assetUserRole.Role.ToString()
        };
    }
}