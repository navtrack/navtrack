using System.Text.Json;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Navtrack.DataAccess.Model.Settings;
using Navtrack.DataAccess.Mongo;
using Navtrack.Library.DI;

namespace Navtrack.DataAccess.Services.Settings;

[Service(typeof(ISettingDataService))]
public class SettingDataService : ISettingDataService
{
    private readonly IRepository repository;

    public SettingDataService(IRepository repository)
    {
        this.repository = repository;
    }

    public async Task<string> GetSetting(string key)
    {
        SettingDocument setting =
            await repository.GetEntities<SettingDocument>().FirstOrDefaultAsync(x => x.Key == key);

        return setting?.Value;
    }

    public async Task<T> GetSetting<T>(string key)
    {
        SettingDocument setting =
            await repository.GetEntities<SettingDocument>()
                .FirstOrDefaultAsync(x => x.Key == key);

        return setting != null && !string.IsNullOrEmpty(setting.Value)
            ? JsonSerializer.Deserialize<T>(setting.Value)
            : default;
    }

    public Task SaveSetting<T>(string key, T value)
    {
        return repository.GetCollection<SettingDocument>()
            .ReplaceOneAsync(x => x.Key == key, new SettingDocument
            {
                Id = ObjectId.GenerateNewId(),
                Key = key,
                Value = JsonSerializer.Serialize(value)
            }, new ReplaceOptions { IsUpsert = true });
    }

    public Task SetSetting(string key, string value)
    {
        return repository.GetCollection<SettingDocument>()
            .ReplaceOneAsync(x => x.Key == key, new SettingDocument
            {
                Id = ObjectId.GenerateNewId(),
                Key = key,
                Value = value
            }, new ReplaceOptions { IsUpsert = true });
    }
}