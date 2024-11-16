using Navtrack.Api.Model.User;
using Navtrack.DataAccess.Model.Users;

namespace Navtrack.Api.Services.User.Mappers;

public static class UserAssetMapper
{
    public static UserAsset Map(UserAssetElement source)
    {
        return new UserAsset
        {
            AssetId = source.AssetId.ToString(),
            UserRole = source.UserRole
        };
    }
}