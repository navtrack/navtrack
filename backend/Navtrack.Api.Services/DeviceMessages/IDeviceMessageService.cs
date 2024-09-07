using System.Threading.Tasks;
using Navtrack.Api.Model.Messages;

namespace Navtrack.Api.Services.DeviceMessages;

public interface IDeviceMessageService
{
    Task<MessageListModel> GetPositions(string assetId, MessageFilterModel messageFilter, int page, int size);
}