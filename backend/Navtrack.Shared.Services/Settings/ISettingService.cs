using System.Threading.Tasks;

namespace Navtrack.Common.Settings;

public interface ISettingService
{
    Task<T?> Get<T>() where T : new();
    Task Set<T>(T settings);
}