using System.Threading.Tasks;

namespace Navtrack.Listener.Services;

public interface IDeviceMessageService
{
    Task<SaveDeviceMessageResult?> Save(SaveDeviceMessageInput input);
}