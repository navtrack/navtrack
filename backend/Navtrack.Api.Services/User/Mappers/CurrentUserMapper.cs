using System.Linq;
using Navtrack.Api.Model.User;
using Navtrack.Database.Model.Users;

namespace Navtrack.Api.Services.User.Mappers;

public static class CurrentUserMapper
{
    public static CurrentUserModel Map(UserEntity? source, CurrentUserModel? destination = null)
    {
        destination ??= new CurrentUserModel();

        destination.Id = source.Id.ToString();
        destination.Email = source.Email;
        destination.Units = source.UnitsType;
        destination.Assets = source.AssetUsers.Select(UserAssetMapper.Map).ToList();
        destination.Organizations = source.OrganizationUsers.Select(UserOrganizationMapper.Map).ToList();
        destination.Authentication = UserAuthenticationMapper.Map(source);

        return destination;
    }
}