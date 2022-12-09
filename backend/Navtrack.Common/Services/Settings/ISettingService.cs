using System.Collections.Generic;
using System.Threading.Tasks;

namespace Navtrack.Common.Services.Settings;

public interface ISettingService
{
    Task<Dictionary<string, string>> GetPublicSettings();
    Task<T> Get<T>() where T : new();
    Task Set<T>(T settings);
}