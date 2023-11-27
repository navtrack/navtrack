using System.Threading.Tasks;
using MongoDB.Bson;
using Navtrack.DataAccess.Model.Settings;

namespace Navtrack.DataAccess.Services.Settings;

public interface ISettingRepository
{
    Task<SettingDocument?> Get(string key);
    Task Save(string key, BsonDocument value);
}