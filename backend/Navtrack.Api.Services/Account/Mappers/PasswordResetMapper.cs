using System;
using System.Security.Cryptography;
using System.Text;
using MongoDB.Bson;
using Navtrack.DataAccess.Model.Users.PasswordResets;

namespace Navtrack.Api.Services.Account.Mappers;

public static class PasswordResetMapper
{
    public static PasswordResetDocument Map(string email, ObjectId userId, string ipAddress)
    {
        PasswordResetDocument document = new()
        {
            Email = email,
            CreatedDate = DateTime.UtcNow,
            CreatedBy = userId,
            Hash = Convert.ToHexString(MD5.HashData(Encoding.UTF8.GetBytes(Guid.NewGuid().ToString()))).ToLower(),
            IpAddress = ipAddress
        };

        return document;
    }
}