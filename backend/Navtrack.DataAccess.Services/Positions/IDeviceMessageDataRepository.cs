using Navtrack.DataAccess.Model.Devices.Messages;
using Navtrack.DataAccess.Mongo;

namespace Navtrack.DataAccess.Services.Positions;

public interface IDeviceMessageDataRepository : IGenericRepository<DeviceMessageDataDocument>;