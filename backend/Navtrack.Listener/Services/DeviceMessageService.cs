using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Navtrack.DataAccess.Model.Devices.Messages;
using Navtrack.DataAccess.Services.Assets;
using Navtrack.DataAccess.Services.Devices;
using Navtrack.Listener.Models;
using Navtrack.Listener.Services.Mappers;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Services;

[Service(typeof(IDeviceMessageService))]
public class DeviceMessageService(
    IDeviceMessageRepository deviceMessageRepository,
    IAssetRepository assetRepository,
    IDeviceMessageDataRepository deviceMessageDataRepository) : IDeviceMessageService
{
    public async Task<SaveDeviceMessageResult?> Save(SaveDeviceMessageInput input)
    {
        if (input.Device is { AssetId: not null, DeviceId: not null })
        {
            List<DeviceMessageDocument> mappedMessages = input.Messages
                .Select(x => DeviceMessageDocumentMapper.Map(input.Device, input.ConnectionId, x))
                .OrderBy(x => x.CreatedDate)
                .ToList();

            await deviceMessageRepository.AddRange(mappedMessages);

            await SaveAdditionalData(mappedMessages);
            DateTime? positionDate = await UpdateAssetMessages(input.Device, mappedMessages);

            return new SaveDeviceMessageResult
            {
                MaxPositionDate = positionDate
            };
        }

        return null;
    }

    private async Task<DateTime?> UpdateAssetMessages(Device device, List<DeviceMessageDocument> messages)
    {
        DeviceMessageDocument latestMessage = messages.MaxBy(x => x.CreatedDate)!;
        DeviceMessageDocument latestPositionMessage = messages.MaxBy(x => x.Position.Date)!;

        bool updatePosition = (device.MaxDate == null || device.MaxDate < latestPositionMessage.Position.Date) &&
                              latestPositionMessage.Position.Latitude != 0 &&
                              latestPositionMessage.Position.Longitude != 0;

        if (updatePosition)
        {
            device.MaxDate = latestPositionMessage.Position.Date;
        }

        await assetRepository.UpdateMessages(device.AssetId!.Value, latestMessage,
            updatePosition ? latestPositionMessage : null);

        if (updatePosition)
        {
            return latestPositionMessage.Position.Date;
        }

        return null;
    }

    private async Task SaveAdditionalData(List<DeviceMessageDocument> mappedMessages)
    {
        IEnumerable<DeviceMessageDataDocument> data = DeviceMessageDataDocumentMapper.Select(mappedMessages);

        await deviceMessageDataRepository.AddRange(data);
    }
}