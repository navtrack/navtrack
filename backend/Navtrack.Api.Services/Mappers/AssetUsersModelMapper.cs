using System.Collections.Generic;
using System.Linq;
using Navtrack.Api.Model.Assets;
using Navtrack.DataAccess.Model.Assets;
using Navtrack.DataAccess.Model.Users;

namespace Navtrack.Api.Services.Mappers;

public static class AssetUsersModelMapper
{
    public static AssetUserListModel Map(AssetDocument asset, List<UserDocument> users)
    {
        AssetUserListModel list = new()
        {
            Items = asset.UserRoles
                .Select(x =>
                    AssetUserModelMapper.Map(x, users.First(y => x.UserId == y.Id)))
                .ToList()
        };

        return list;
    }
}