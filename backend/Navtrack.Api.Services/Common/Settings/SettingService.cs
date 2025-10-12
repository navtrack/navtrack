using System;
using System.Threading.Tasks;
using Navtrack.Database.Model;
using Navtrack.Database.Services.Settings;
using Navtrack.Shared.Library.DI;
using Newtonsoft.Json;

namespace Navtrack.Api.Services.Common.Settings;

[Service(typeof(ISettingService))]
public class SettingService(ISettingRepository repository) : ISettingService
{
    public async Task<T?> Get<T>() where T : new()
    {
        string key = GetKey<T>();

        SystemSettingEntity? document = await repository.Get(key);

        if (document != null)
        {
            return JsonConvert.DeserializeObject<T>(document.Value);
        }

        await Set(new T());

        return default;
    }

    public async Task Set<T>(T settings)
    {
        string key = GetKey<T>();

        await repository.Save(key, JsonConvert.SerializeObject(settings));
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