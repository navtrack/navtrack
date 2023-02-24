using System;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using Navtrack.DataAccess.Model.Settings;
using Navtrack.DataAccess.Services.Settings;
using Navtrack.Library.DI;

namespace Navtrack.Common.Settings;

[Service(typeof(ISettingService))]
public class SettingService : ISettingService
{
    private readonly ISettingDataService settingDataService;

    public SettingService(ISettingDataService settingDataService)
    {
        this.settingDataService = settingDataService;
    }

    public async Task<T?> Get<T>() where T : new()
    {
        string key = GetKey<T>();

        SettingDocument? document = await settingDataService.Get(key);

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

        await settingDataService.Save(key, settings.ToBsonDocument());
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