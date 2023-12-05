using Navtrack.Api.Model.User;
using Navtrack.DataAccess.Model.Users;

namespace Navtrack.Api.Services.Mappers.Users;

public static class UserMapper
{
    public static UserModel Map(UserDocument entity, UserModel? destination = null)
    {
        destination ??= new UserModel();

        destination.Id = entity.Id.ToString();
        destination.Email = entity.Email;
        destination.Units = entity.UnitsType;

        return destination;
    }
}