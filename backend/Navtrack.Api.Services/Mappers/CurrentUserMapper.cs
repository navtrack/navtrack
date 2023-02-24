using Navtrack.Api.Model.User;
using Navtrack.Core.Model;
using Navtrack.DataAccess.Model.Common;
using Navtrack.DataAccess.Model.Users;

namespace Navtrack.Api.Services.Mappers;

internal static class CurrentUserMapper
{
    public static CurrentUserModel Map(UserDocument entity)
    {
        return new CurrentUserModel
        {
            Id = entity.Id.ToString(),
            Email = entity.Email,
            Units = (UnitsType)entity.UnitsType
        };
    }
}