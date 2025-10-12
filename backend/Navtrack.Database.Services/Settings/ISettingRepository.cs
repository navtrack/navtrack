using System.Threading.Tasks;
using Navtrack.Database.Model;

namespace Navtrack.Database.Services.Settings;

public interface ISettingRepository
{
    Task<SystemSettingEntity?> Get(string key);
    Task Save(string key, string value);
}