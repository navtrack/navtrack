using Navtrack.DataAccess.Model.Devices.Messages;
using Navtrack.DataAccess.Mongo;

namespace Navtrack.DataAccess.Services.Devices;

public interface IDeviceMessageDataRepository : IGenericRepository<DeviceMessageDataDocument>;