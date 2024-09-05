using System.Collections.Generic;
using System.Linq;
using Navtrack.DataAccess.Model.Devices.Messages;

namespace Navtrack.Listener.Mappers;

public static class DeviceMessageDataDocumentMapper
{
    public static IEnumerable<DeviceMessageDataDocument> Select(List<DeviceMessageDocument> mappedMessages)
    {
        return mappedMessages.Select(x => new DeviceMessageDataDocument
        {
            DeviceMessageId = x.Id,
            Data = x.AdditionalData,
            DataUnhandled = x.AdditionalDataUnhandled
        });
    }
}