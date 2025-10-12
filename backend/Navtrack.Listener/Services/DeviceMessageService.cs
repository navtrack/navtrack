using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Navtrack.Database.Model.Devices;
using Navtrack.Database.Services.Assets;
using Navtrack.Database.Services.Devices;
using Navtrack.Listener.Models;
using Navtrack.Listener.Services.Mappers;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Services;

[Service(typeof(IDeviceMessageService))]
public class DeviceMessageService(
    IDeviceMessageRepository deviceMessageRepository,
    IAssetRepository assetRepository) : IDeviceMessageService
{
    public async Task<SaveDeviceMessageResult?> Save(SaveDeviceMessageInput input)
    {
        if (input.Device is { AssetId: not null, DeviceId: not null })
        {
            List<DeviceMessageEntity> mappedMessages = input.Messages
                .Select(x => DeviceMessageDocumentMapper.Map(input.Device.AssetId.Value, 
                    input.Device.DeviceId.Value,
                    input.ConnectionId, x))
                .OrderBy(x => x.CreatedDate)
                .ToList();

            await deviceMessageRepository.AddRange(mappedMessages);

            DateTime? positionDate = await UpdateAssetMessages(input.Device, mappedMessages);

            return new SaveDeviceMessageResult
            {
                MaxPositionDate = positionDate
            };
        }

        return null;
    }

    private async Task<DateTime?> UpdateAssetMessages(Device device, List<DeviceMessageEntity> messages)
    {
        DeviceMessageEntity latestMessage = messages.MaxBy(x => x.CreatedDate)!;
        DeviceMessageEntity latestPositionMessage = messages.MaxBy(x => x.Date)!;

        bool updatePosition = (device.MaxDate == null || device.MaxDate < latestPositionMessage.Date) &&
                              latestPositionMessage.Latitude != 0 &&
                              latestPositionMessage.Longitude != 0;

        if (updatePosition)
        {
            device.MaxDate = latestPositionMessage.Date;
        }

        await assetRepository.UpdateMessages(device.AssetId.Value, latestMessage.Id,
            updatePosition ? latestPositionMessage.Id : null);

        if (updatePosition)
        {
            return latestPositionMessage.Date;
        }

        return null;
    }
}