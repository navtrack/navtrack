using System.Collections.Generic;
using Navtrack.Api.Model.Devices;

namespace Navtrack.Api.Services.Devices
{
    public interface IDeviceTypeService
    {
        List<DeviceTypeModel> GetDeviceTypes();
    }
}