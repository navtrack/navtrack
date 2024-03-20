using System;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using Navtrack.DataAccess.Model.System;
using Navtrack.DataAccess.Services.Settings;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Shared.Services.Settings;

[Service(typeof(ISettingService))]
public class SettingService(ISettingRepository repository) : ISettingService
{
    public async Task<T?> Get<T>() where T : new()
    {
        string key = GetKey<T>();

        SystemSettingDocument? document = await repository.Get(key);

        if (document != null)
        {
            return BsonSerializer.Deserialize<T>(document.Value);
        }

        await Set(new T());

        return default;
    }

    public async Task Set<T>(T settings)
    {
        string key = GetKey<T>();

        await repository.Save(key, settings.ToBsonDocument());
    }

    private static string GetKey<T>()
    {
        if (!typeof(T).Name.EndsWith("Settings"))
        {
            throw new ArgumentException("Provided type name must end with 'Settings'.");
        }

        string key = typeof(T).Name.Replace("Settings", string.Empty);

        return key;
    }
}