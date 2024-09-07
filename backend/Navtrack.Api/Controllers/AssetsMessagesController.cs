using Navtrack.Api.Services.DeviceMessages;
using Navtrack.Api.Shared.Controllers;

namespace Navtrack.Api.Controllers;

public class AssetsMessagesController(IDeviceMessageService deviceMessageService)
    : AssetsMessagesControllerBase(deviceMessageService);