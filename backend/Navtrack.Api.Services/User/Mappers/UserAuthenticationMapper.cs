using Navtrack.Api.Model.User;
using Navtrack.Database.Model.Users;

namespace Navtrack.Api.Services.User.Mappers;

public static class UserAuthenticationMapper
{
    public static UserAuthenticationModel Map(UserEntity source, UserAuthenticationModel? destination = null)
    {
        destination ??= new UserAuthenticationModel();
        
        destination.Password = source.PasswordHash != null;

        return destination;
    }
}