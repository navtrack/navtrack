using Navtrack.Api.Services.Devices;
using Navtrack.Api.Shared.Controllers;

namespace Navtrack.Api.Controllers;

public class AssetsDevicesController : AssetsDevicesControllerBase
{
    public AssetsDevicesController(IDeviceService deviceService) : base(deviceService)
    {
    }
}