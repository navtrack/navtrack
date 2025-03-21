using System.Collections.Generic;
using Navtrack.DataAccess.Model.Devices;

namespace Navtrack.DataAccess.Services.Devices;

public interface IDeviceTypeRepository
{
    DeviceType? GetById(string deviceTypeId);
    IEnumerable<DeviceType> GetDeviceTypes();
}