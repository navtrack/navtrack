using Navtrack.DataAccess.Model.Common;
using Navtrack.DataAccess.Model.Users;

namespace Navtrack.Api.Services.Mappers;

public static class UserDocumentMapper
{
    public static UserDocument Map(string email, string hash, string salt)
    {
        return new UserDocument
        {
            Email = email,
            Password = new PasswordElement
            {
                Hash = hash,
                Salt = salt
            },
            UnitsType = UnitsType.Metric,
            Created = AuditElementMapper.Map(),
        };
    }
}