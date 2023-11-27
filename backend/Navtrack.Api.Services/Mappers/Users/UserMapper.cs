using Navtrack.DataAccess.Model.Users;

namespace Navtrack.Api.Services.Mappers.Users;

public static class UserMapper
{
    public static Model.User.UserModel Map(UserDocument entity, Model.User.UserModel? destination = null)
    {
        destination ??= new Model.User.UserModel();

        destination.Id = entity.Id.ToString();
        destination.Email = entity.Email;
        destination.Units = entity.UnitsType;

        return destination;
    }
}