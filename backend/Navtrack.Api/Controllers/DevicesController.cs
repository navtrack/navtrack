using Navtrack.Api.Services.Devices;
using Navtrack.Api.Shared.Controllers;

namespace Navtrack.Api.Controllers;

public class DevicesController(IDeviceTypeService deviceTypeService) : DevicesControllerBase(deviceTypeService);