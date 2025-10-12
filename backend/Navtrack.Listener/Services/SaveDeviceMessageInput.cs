using System;
using System.Collections.Generic;
using Navtrack.Database.Model.Devices;
using Navtrack.Listener.Models;

namespace Navtrack.Listener.Services;

public class SaveDeviceMessageInput
{
    public Guid ConnectionId { get; set; }
    public Device Device { get; set; }
    public IEnumerable<DeviceMessageEntity> Messages { get; set; }
}