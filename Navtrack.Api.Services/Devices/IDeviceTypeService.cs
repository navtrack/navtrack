using Navtrack.Api.Model.Devices;

namespace Navtrack.Api.Services.Devices;

public interface IDeviceTypeService
{
    DeviceTypesModel GetAll();
}