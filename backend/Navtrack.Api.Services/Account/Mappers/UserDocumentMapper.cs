using System;
using Navtrack.DataAccess.Model.Shared;
using Navtrack.DataAccess.Model.Users;
using Navtrack.Shared.Library.Extensions;

namespace Navtrack.Api.Services.Account.Mappers;

public static class UserDocumentMapper
{
    public static UserDocument Map(string email, string hash, string salt)
    {
        UserDocument userDocument = Map(email);
        
        userDocument.Password = new PasswordElement
        {
            Hash = hash,
            Salt = salt
        };

        return userDocument;
    }

    public static UserDocument Map(string email, UserDocument? destination = null)
    {
        UserDocument userDocument = destination ?? new UserDocument();

        userDocument.Email = email.TrimAndLower();
        userDocument.UnitsType = UnitsType.Metric;
        userDocument.CreatedDate = DateTime.UtcNow;

        return userDocument;
    }
}