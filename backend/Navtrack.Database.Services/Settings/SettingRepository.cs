using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Navtrack.Database.Model;
using Navtrack.Database.Postgres;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Database.Services.Settings;

[Service(typeof(ISettingRepository))]
public class SettingRepository(IPostgresRepository repository) : ISettingRepository
{
    public async Task<SystemSettingEntity?> Get(string key)
    {
        SystemSettingEntity? document =
            await repository.GetQueryable<SystemSettingEntity>()
                .FirstOrDefaultAsync(x => x.Key == key);

        return document;
    }

    public async Task Save(string key, string value)
    {
        SystemSettingEntity? document = await Get(key);

        if (document == null)
        {
            document = new SystemSettingEntity
            {
                Key = key,
                Value = value
            };
            repository.GetQueryable<SystemSettingEntity>().Add(document);
        }
        else
        {
            document.Value = value;

            repository.GetQueryable<SystemSettingEntity>().Update(document);
        }

        await repository.GetDbContext().SaveChangesAsync();
    }
}