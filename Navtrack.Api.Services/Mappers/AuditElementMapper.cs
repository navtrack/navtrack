using System;
using MongoDB.Bson;
using Navtrack.DataAccess.Model.Common;

namespace Navtrack.Api.Services.Mappers;

public static class AuditElementMapper
{
    public static AuditElement Map(ObjectId? userId = null)
    {
        return new AuditElement
        {
            Date = DateTime.UtcNow,
            UserId = userId
        };
    }
}