using System.Collections.Generic;
using System.Threading.Tasks;
using Navtrack.Web.Model.Models;

namespace Navtrack.Web.Services
{
    public interface IDeviceService
    {
        Task<List<DeviceModel>> Get();
        Task Add(DeviceModel deviceModel);
        Task<bool> IsValidNewDevice(DeviceModel deviceModel);
    }
}