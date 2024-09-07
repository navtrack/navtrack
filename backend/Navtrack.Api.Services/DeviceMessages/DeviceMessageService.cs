using System.Threading.Tasks;
using Navtrack.Api.Model.Messages;
using Navtrack.Api.Services.DeviceMessages.Mappers;
using Navtrack.DataAccess.Model.Devices.Messages.Filters;
using Navtrack.DataAccess.Services.Devices;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Api.Services.DeviceMessages;

[Service(typeof(IDeviceMessageService))]
public class DeviceMessageService(IDeviceMessageRepository deviceMessageRepository) : IDeviceMessageService
{
    public async Task<MessageListModel> GetPositions(string assetId, MessageFilterModel messageFilter, int page,
        int size)
    {
        GetMessagesResult messages = await deviceMessageRepository.GetMessages(new GetMessagesOptions
        {
            AssetId = assetId,
            PositionFilter = messageFilter,
            Page = page,
            Size = size
        });

        MessageListModel model = MessageListModelMapper.Map(messages);

        return model;
    }
}