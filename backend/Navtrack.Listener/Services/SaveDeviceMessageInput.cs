using System.Collections.Generic;
using MongoDB.Bson;
using Navtrack.DataAccess.Model.Devices.Messages;
using Navtrack.Listener.Models;

namespace Navtrack.Listener.Services;

public class SaveDeviceMessageInput
{
    public ObjectId ConnectionId { get; set; }
    public Device Device { get; set; }
    public IEnumerable<DeviceMessageDocument> Messages { get; set; }
}