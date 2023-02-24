using System.Collections.Generic;
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

    public async Task<SettingDocument?> Get(string key)
    {
        SettingDocument? document =
            await repository.GetEntities<SettingDocument>().FirstOrDefaultAsync(x => x.Key == key);

        return document;
    }

    public async Task Save(string key, BsonDocument value)
    {
        SettingDocument? document = await Get(key);

        if (document == null)
        {
            document = new SettingDocument
            {
                Id = ObjectId.GenerateNewId(),
                Key = key,
                Value = value
            };
        }
        else
        {
            document.Value = value;
        }

        await repository.GetCollection<SettingDocument>()
            .ReplaceOneAsync(x => x.Id == document.Id, document, new ReplaceOptions
            {
                IsUpsert = true
            });
    }

    public Task<List<SettingDocument>> GetAll()
    {
        return repository.GetEntities<SettingDocument>().ToListAsync();
    }
}