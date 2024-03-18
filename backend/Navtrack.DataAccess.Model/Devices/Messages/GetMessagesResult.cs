using System.Collections.Generic;

namespace Navtrack.DataAccess.Model.Devices.Messages;

public class GetMessagesResult
{
    public required long TotalCount { get; init; }
    public required List<DeviceMessageDocument> Messages { get; init; }
}