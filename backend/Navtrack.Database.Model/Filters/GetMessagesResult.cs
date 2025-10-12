using System.Collections.Generic;
using Navtrack.Database.Model.Devices;

namespace Navtrack.Database.Model.Filters;

public class GetMessagesResult
{
    public required long TotalCount { get; init; }
    public required List<DeviceMessageEntity> Messages { get; init; }
}