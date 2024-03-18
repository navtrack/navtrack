using System.Threading.Tasks;
using MongoDB.Bson;
using Navtrack.DataAccess.Model.System;

namespace Navtrack.DataAccess.Services.Settings;

public interface ISettingRepository
{
    Task<SystemSettingDocument?> Get(string key);
    Task Save(string key, BsonDocument value);
}