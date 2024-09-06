using System.Threading.Tasks;
using Navtrack.Api.Model.Messages;
using Navtrack.Api.Services.Mappers.Messages;
using Navtrack.DataAccess.Model.Devices.Messages.Filters;
using Navtrack.DataAccess.Services.Positions;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Api.Services.Positions;

[Service(typeof(IPositionService))]
public class PositionService(IDeviceMessageRepository deviceMessageRepository) : IPositionService
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