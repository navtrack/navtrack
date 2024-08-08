using System.Linq;
using Navtrack.Api.Model.User;
using Navtrack.DataAccess.Model.Users;

namespace Navtrack.Api.Services.Mappers.Users;

public static class UserMapper
{
    public static UserModel Map(UserDocument source, UserModel? destination = null)
    {
        destination ??= new UserModel();

        destination.Id = source.Id.ToString();
        destination.Email = source.Email;
        destination.Units = source.UnitsType;
        destination.AssetRoles = source.AssetRoles?.Select(UserAssetRoleModelMapper.Map).ToList() ?? [];
        destination.Authentication = UserAuthenticationModelMapper.Map(source);

        return destination;
    }
}