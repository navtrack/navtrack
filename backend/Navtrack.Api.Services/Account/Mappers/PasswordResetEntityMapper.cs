using System;
using Navtrack.Database.Model.Users;

namespace Navtrack.Api.Services.Account.Mappers;

public static class PasswordResetEntityMapper
{
    public static UserPasswordResetEntity Map(string email, Guid userId, string ipAddress)
    {
        UserPasswordResetEntity entity = new()
        {
            Email = email,
            CreatedDate = DateTime.UtcNow,
            CreatedBy = userId,
            IpAddress = ipAddress
        };

        return entity;
    }
}