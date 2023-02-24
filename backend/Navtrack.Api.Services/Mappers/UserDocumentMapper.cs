using Navtrack.Core.Model;
using Navtrack.DataAccess.Model.Common;
using Navtrack.DataAccess.Model.Users;

namespace Navtrack.Api.Services.Mappers;

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

    public static UserDocument MapWithExternalId(string email, string? microsoftId = null, string? googleId = null,
        string? appleId = null)
    {
        UserDocument userDocument = Map(email);
        userDocument.MicrosoftId = microsoftId;
        userDocument.GoogleId = googleId;
        userDocument.AppleId = appleId;

        return userDocument;
    }

    private static UserDocument Map(string email)
    {
        return new UserDocument
        {
            Email = email.ToLower(),
            UnitsType = UnitsType.Metric,
            Created = AuditElementMapper.Map(),
        };
    }
}