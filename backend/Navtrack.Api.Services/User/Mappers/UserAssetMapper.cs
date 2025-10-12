using Navtrack.Api.Model.User;
using Navtrack.Database.Model.Assets;

namespace Navtrack.Api.Services.User.Mappers;

public static class UserAssetMapper
{
    public static UserAssetModel Map(AssetUserEntity source)
    {
        return new UserAssetModel
        {
            AssetId = source.AssetId.ToString(),
            UserRole = source.UserRole
        };
    }
}