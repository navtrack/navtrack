using Navtrack.Api.Model.Common;
using Navtrack.Api.Model.Devices;

namespace Navtrack.Api.Services.Devices;

public interface IDeviceTypeService
{
    ListModel<DeviceTypeModel> GetAll();
}