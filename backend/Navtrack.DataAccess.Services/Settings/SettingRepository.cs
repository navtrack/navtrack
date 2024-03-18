using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Navtrack.DataAccess.Model.System;
using Navtrack.DataAccess.Mongo;
using Navtrack.Shared.Library.DI;

namespace Navtrack.DataAccess.Services.Settings;

[Service(typeof(ISettingRepository))]
public class SettingRepository(IRepository repository) : ISettingRepository
{
    public async Task<SystemSettingDocument?> Get(string key)
    {
        SystemSettingDocument? document =
            await repository.GetQueryable<SystemSettingDocument>().FirstOrDefaultAsync(x => x.Key == key);

        return document;
    }

    public async Task Save(string key, BsonDocument value)
    {
        SystemSettingDocument? document = await Get(key);

        if (document == null)
        {
            document = new SystemSettingDocument
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

        await repository.GetCollection<SystemSettingDocument>()
            .ReplaceOneAsync(x => x.Id == document.Id, document, new ReplaceOptions
            {
                IsUpsert = true
            });
    }
}