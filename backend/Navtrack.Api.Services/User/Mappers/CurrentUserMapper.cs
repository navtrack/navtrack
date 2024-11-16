using System.Linq;
using Navtrack.Api.Model.User;
using Navtrack.DataAccess.Model.Users;

namespace Navtrack.Api.Services.User.Mappers;

public static class CurrentUserMapper
{
    public static CurrentUser Map(UserDocument source, CurrentUser? destination = null)
    {
        destination ??= new CurrentUser();

        destination.Id = source.Id.ToString();
        destination.Email = source.Email;
        destination.Units = source.UnitsType;
        destination.Assets = source.Assets?.Select(UserAssetMapper.Map).ToList() ?? [];
        destination.Organizations = source.Organizations?.Select(UserOrganizationMapper.Map).ToList() ?? [];
        destination.Authentication = UserAuthenticationMapper.Map(source);

        return destination;
    }
}