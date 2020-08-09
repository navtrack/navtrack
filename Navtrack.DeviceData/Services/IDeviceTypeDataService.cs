using System.Collections.Generic;
using Navtrack.DeviceData.Model;

namespace Navtrack.DeviceData.Services
{
    public interface IDeviceTypeDataService
    {
        DeviceType GetById(int deviceTypeId);
        bool Exists(int deviceTypeId);
        IEnumerable<DeviceType> GetDeviceTypes();
    }
}