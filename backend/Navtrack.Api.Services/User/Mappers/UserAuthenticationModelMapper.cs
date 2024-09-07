using Navtrack.Api.Model.User;
using Navtrack.DataAccess.Model.Users;

namespace Navtrack.Api.Services.User.Mappers;

public static class UserAuthenticationModelMapper
{
    public static UserAuthenticationModel Map(UserDocument source, UserAuthenticationModel? destination = null)
    {
        destination ??= new UserAuthenticationModel();
        
        destination.Password = source.Password != null;

        return destination;
    }
}