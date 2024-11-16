using System.Threading.Tasks;

namespace Navtrack.Api.Services.Common.Settings;

public interface ISettingService
{
    Task<T?> Get<T>() where T : new();
    Task Set<T>(T settings);
}