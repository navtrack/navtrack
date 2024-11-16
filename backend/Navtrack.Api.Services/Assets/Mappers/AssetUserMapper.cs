using System.Linq;
using Navtrack.Api.Model.Assets;
using Navtrack.DataAccess.Model.Assets;
using Navtrack.DataAccess.Model.Users;

namespace Navtrack.Api.Services.Assets.Mappers;

public static class AssetUserMapper
{
    public static AssetUser Map(UserDocument user, AssetDocument source)
    {
        UserAssetElement assetUser = user.Assets!.First(x => x.AssetId == source.Id);
        
        return new AssetUser
        {
            Email = user.Email,
            UserId = user.Id.ToString(),
            UserRole = assetUser.UserRole,
            CreatedDate = source.CreatedDate
        };
    }
}