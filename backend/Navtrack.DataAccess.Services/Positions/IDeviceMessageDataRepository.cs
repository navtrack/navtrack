using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using Navtrack.DataAccess.Model.Devices.Messages;
using Navtrack.DataAccess.Model.Devices.Messages.Filters;
using Navtrack.DataAccess.Mongo;

namespace Navtrack.DataAccess.Services.Positions;

public interface IDeviceMessageDataRepository : IGenericRepository<DeviceMessageDataDocument>
{
}