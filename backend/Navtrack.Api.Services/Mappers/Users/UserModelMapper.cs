using Navtrack.Api.Model.User;
using Navtrack.DataAccess.Model.Users;

namespace Navtrack.Api.Services.Mappers.Users;

internal static class UserModelMapper
{
    public static UserModel Map(UserDocument entity)
    {
        return new UserModel
        {
            Id = entity.Id.ToString(),
            Email = entity.Email,
            Units = entity.UnitsType
        };
    }
}