using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Navtrack.Database.Model;
using Navtrack.Database.Services.Settings;
using Navtrack.Shared.Library.DI;
using Newtonsoft.Json;

namespace Navtrack.Api.Services.Common.Settings;

[Service(typeof(ISettingService), ServiceLifetime.Singleton)]
public class SettingService(IServiceProvider serviceProvider) : ISettingService
{
    private Dictionary<string, object> cache = new();
    
    public async Task<T?> Get<T>() where T : new()
    {
        string key = GetKey<T>();
        
        if (cache.TryGetValue(key, out object? value))
        {
            return (T)value;
        }

        using IServiceScope scope = serviceProvider.CreateScope();
        ISettingRepository repository = scope.ServiceProvider.GetRequiredService<ISettingRepository>();
        SystemSettingEntity? document = await repository.Get(key);

        if (document != null)
        {
            T deserializeObject = JsonConvert.DeserializeObject<T>(document.Value);
            
            cache[key] = deserializeObject;
            
            return deserializeObject;
        }

        await Set(new T());

        return default;
    }

    public async Task Set<T>(T settings)
    {
        string key = GetKey<T>();

        using IServiceScope scope = serviceProvider.CreateScope();
        ISettingRepository repository = scope.ServiceProvider.GetRequiredService<ISettingRepository>();
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