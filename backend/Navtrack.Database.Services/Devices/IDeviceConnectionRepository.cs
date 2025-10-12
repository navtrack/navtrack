using System;
using System.Threading.Tasks;
using Navtrack.Database.Model.Devices;
using Navtrack.Database.Postgres;

namespace Navtrack.Database.Services.Devices;

public interface IDeviceConnectionRepository : IGenericPostgresRepository<DeviceConnectionEntity>
{
    Task AddMessage(Guid connectionId, byte[] hex);
}