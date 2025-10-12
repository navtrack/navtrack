using System.Collections.Generic;
using Navtrack.Database.Model.Devices;

namespace Navtrack.Database.Services.Devices;

public interface IDeviceTypeRepository
{
    DeviceType? GetById(string deviceTypeId);
    IEnumerable<DeviceType> GetDeviceTypes();
}