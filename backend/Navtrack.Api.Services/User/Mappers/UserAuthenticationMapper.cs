using Navtrack.Api.Model.User;
using Navtrack.DataAccess.Model.Users;

namespace Navtrack.Api.Services.User.Mappers;

public static class UserAuthenticationMapper
{
    public static UserAuthentication Map(UserDocument source, UserAuthentication? destination = null)
    {
        destination ??= new UserAuthentication();
        
        destination.Password = source.Password != null;

        return destination;
    }
}