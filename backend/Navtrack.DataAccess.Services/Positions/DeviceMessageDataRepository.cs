using Navtrack.DataAccess.Model.Devices.Messages;
using Navtrack.DataAccess.Mongo;
using Navtrack.Shared.Library.DI;

namespace Navtrack.DataAccess.Services.Positions;

[Service(typeof(IDeviceMessageDataRepository))]
public class DeviceMessageDataRepository(IRepository repository)
    : GenericRepository<DeviceMessageDataDocument>(repository), IDeviceMessageDataRepository;