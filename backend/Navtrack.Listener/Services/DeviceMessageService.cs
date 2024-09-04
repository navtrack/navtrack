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

[Service(typeof(IDeviceMessageService))]
public class DeviceMessageService(IDeviceMessageRepository deviceMessageRepository, IAssetRepository assetRepository)
    : IDeviceMessageService
{
    public async Task Save(ObjectId connectionId, Device device, IEnumerable<DeviceMessageDocument> positions)
    {
        if (device is { AssetId: not null, DeviceId: not null })
        {
            List<DeviceMessageDocument> messages = positions
                .Select(x => MessageDocumentMapper.Map(device, connectionId, x))
                .OrderBy(x => x.CreatedDate)
                .ToList();

            await deviceMessageRepository.AddRange(messages);

            DeviceMessageDocument? latestPositionMessage = messages.MaxBy(x => x.Position.Date);

            if (latestPositionMessage != null && (device.MaxDate == null || device.MaxDate < latestPositionMessage.Position.Date) &&
                latestPositionMessage.Position.Latitude != 0 && latestPositionMessage.Position.Longitude != 0)
            {
                await assetRepository.SetLastPositionMessage(device.AssetId.Value, latestPositionMessage);
                device.MaxDate = latestPositionMessage.Position.Date;
            }
        }
    }
}