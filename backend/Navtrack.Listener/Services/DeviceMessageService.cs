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
public class DeviceMessageService(
    IDeviceMessageRepository deviceMessageRepository,
    IAssetRepository assetRepository,
    IDeviceMessageDataRepository deviceMessageDataRepository) : IDeviceMessageService
{
    public async Task Save(ObjectId connectionId, Device device, IEnumerable<DeviceMessageDocument> messages)
    {
        if (device is { AssetId: not null, DeviceId: not null })
        {
            List<DeviceMessageDocument> mappedMessages = messages
                .Select(x => DeviceMessageDocumentMapper.Map(device, connectionId, x))
                .OrderBy(x => x.CreatedDate)
                .ToList();

            await deviceMessageRepository.AddRange(mappedMessages);

            IEnumerable<DeviceMessageDataDocument> data = mappedMessages.Select(x => new DeviceMessageDataDocument
            {
                DeviceMessageId = x.Id,
                AdditionalData = x.AdditionalData,
                AdditionalDataUnhandled = x.AdditionalDataUnhandled
            });

            await deviceMessageDataRepository.AddRange(data);

            DeviceMessageDocument latestMessage = mappedMessages.MaxBy(x => x.CreatedDate)!;
            DeviceMessageDocument latestPositionMessage = mappedMessages.MaxBy(x => x.Position.Date)!;

            bool updatePosition = (device.MaxDate == null || device.MaxDate < latestPositionMessage.Position.Date) &&
                                  latestPositionMessage.Position.Latitude != 0 &&
                                  latestPositionMessage.Position.Longitude != 0;

            if (updatePosition)
            {
                device.MaxDate = latestPositionMessage.Position.Date;
            }

            await assetRepository.UpdateMessages(device.AssetId.Value, latestMessage,
                updatePosition ? latestPositionMessage : null);
        }
    }
}