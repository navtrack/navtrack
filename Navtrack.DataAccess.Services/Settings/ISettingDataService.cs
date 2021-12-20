using System.Threading.Tasks;

namespace Navtrack.DataAccess.Services.Settings;

public interface ISettingDataService
{
    Task<string> GetSetting(string key);
    Task<T> GetSetting<T>(string key);
    Task SaveSetting<T>(string key, T value);
    Task SetSetting(string key, string value);
}