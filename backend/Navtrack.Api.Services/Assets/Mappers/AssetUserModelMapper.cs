using Navtrack.Api.Model.Assets;
using Navtrack.DataAccess.Model.Assets;
using Navtrack.DataAccess.Model.Users;

namespace Navtrack.Api.Services.Assets.Mappers;

public static class AssetUserModelMapper
{
    public static AssetUserModel Map(UserDocument user, AssetUserRoleElement source)
    {
        return new AssetUserModel
        {
            Email = user.Email,
            UserId = source.UserId.ToString(),
            Role = source.Role,
            CreatedDate = source.CreatedDate
        };
    }
}