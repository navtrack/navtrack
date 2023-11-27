using System.Collections.Generic;
using System.Linq;
using Navtrack.Api.Model.Assets;
using Navtrack.Api.Model.Common;
using Navtrack.DataAccess.Model.Assets;
using Navtrack.DataAccess.Model.Users;

namespace Navtrack.Api.Services.Mappers.Assets;

public static class AssetUserListModelMapper
{
    public static ListModel<AssetUserModel> Map(AssetDocument asset, List<UserDocument>? users)
    {
        ListModel<AssetUserModel> list = new()
        {
            Items = asset.UserRoles
                .Select(x => new { UserRole = x, User = users?.FirstOrDefault(y => x.UserId == y.Id) })
                .Where(x => x.User != null)
                .Select(x =>
                    AssetUserModelMapper.Map(x.UserRole, x.User))
                .ToList()
        };

        return list;
    }
}