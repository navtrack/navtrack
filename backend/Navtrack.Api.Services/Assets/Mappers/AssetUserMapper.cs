using Navtrack.Api.Model.Assets;
using Navtrack.Database.Model.Assets;

namespace Navtrack.Api.Services.Assets.Mappers;

public static class AssetUserMapper
{
    public static AssetUserModel Map(AssetUserEntity assetUser)
    {
        return new AssetUserModel
        {
            Id = assetUser.Id.ToString(),
            Email = assetUser.User.Email,
            UserId = assetUser.UserId,
            UserRole = assetUser.UserRole,
            CreatedDate = assetUser.CreatedDate
        };
    }
}