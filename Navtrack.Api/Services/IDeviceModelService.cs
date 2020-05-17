using System.Collections.Generic;
using System.Threading.Tasks;
using Navtrack.Api.Models;

namespace Navtrack.Api.Services
{
    public interface IDeviceModelService
    {
        List<DeviceModelModel> GetModels();
    }
}