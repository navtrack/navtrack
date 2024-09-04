using System.Collections.Generic;

namespace Navtrack.DataAccess.Model.Devices.Messages.Filters;

public class GetMessagesResult
{
    public required long TotalCount { get; init; }
    public required List<DeviceMessageDocument> Messages { get; init; }
}