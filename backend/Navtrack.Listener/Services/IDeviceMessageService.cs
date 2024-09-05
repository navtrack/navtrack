using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using Navtrack.DataAccess.Model.Devices.Messages;
using Navtrack.Listener.Models;

namespace Navtrack.Listener.Services;

public interface IDeviceMessageService
{
    Task<SaveDeviceMessageResult?> Save(SaveDeviceMessageInput input);
}