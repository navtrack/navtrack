using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using Navtrack.DataAccess.Model.Settings;
using Navtrack.DataAccess.Services.Settings;
using Navtrack.Library.DI;

namespace Navtrack.Common.Services.Settings;

[Service(typeof(ISettingService))]
public class SettingService : ISettingService
{
    private readonly ISettingDataService settingDataService;

    public SettingService(ISettingDataService settingDataService)
    {
        this.settingDataService = settingDataService;
    }

    public async Task<Dictionary<string, string>> GetPublicSettings()
    {
        List<SettingDocument> settings = await settingDataService.GetSettings();

        Dictionary<string, string> dictionary = new();

        foreach (SettingDocument document in settings.Where(x => x.PublicKeys.Any()))
        {
            foreach (string publicKey in document.PublicKeys)
            {
                BsonElement el = document.Value.FirstOrDefault(x => x.Name == publicKey);

                dictionary.Add($"{document.Key}.{publicKey}", el.Value.ToString());
            }
        }

        return dictionary;
    }

    public async Task<T> Get<T>() where T : new()
    {
        string key = GetKey<T>();

        SettingDocument document = await settingDataService.Get(key);

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