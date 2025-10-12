using System;
using Navtrack.Database.Model.Shared;
using Navtrack.Database.Model.Users;
using Navtrack.Shared.Library.Extensions;

namespace Navtrack.Api.Services.Account.Mappers;

public static class UserDocumentMapper
{
    public static UserEntity Map(string email, string hash, string salt)
    {
        UserEntity user = Map(email);

        user.PasswordHash = hash;
        user.PasswordSalt = salt;

        return user;
    }

    public static UserEntity Map(string email, UserEntity? destination = null)
    {
        UserEntity user = destination ?? new UserEntity();
        
        user.Email = email.TrimAndLower();
        user.UnitsType = UnitsType.Metric;
        user.CreatedDate = DateTime.UtcNow;

        return user;
    }
}