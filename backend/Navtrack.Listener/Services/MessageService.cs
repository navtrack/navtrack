using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using Navtrack.DataAccess.Model.Devices.Messages;
using Navtrack.DataAccess.Services.Assets;
using Navtrack.DataAccess.Services.Positions;
using Navtrack.Listener.Mappers;
using Navtrack.Listener.Models;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Services;

[Service(typeof(IMessageService))]
public class MessageService(IMessageRepository messageRepository, IAssetRepository assetRepository)
    : IMessageService
{
    public async Task Save(ObjectId connectionId, Device device, IEnumerable<Position> positions)
    {
        if (device is { AssetId: not null, DeviceId: not null })
        {
            List<MessageDocument> messages = positions.Select(x => MessageDocumentMapper.Map(x, device, connectionId))
                .OrderBy(x => x.CreatedDate)
                .ToList();

            await messageRepository.AddRange(messages);

            MessageDocument? latestPosition = messages.MaxBy(x => x.Position.Date);

            if (latestPosition != null && (device.MaxDate == null || device.MaxDate < latestPosition.Position.Date))
            {
                await assetRepository.SetLastPositionMessage(device.AssetId.Value, latestPosition);
                device.MaxDate = latestPosition.Position.Date;
            }
        }
    }
}