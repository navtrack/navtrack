using System.Collections.Generic;
using System.Threading.Tasks;
using Navtrack.Web.Models;

namespace Navtrack.Web.Services
{
    public interface IDeviceService
    {
        Task<List<Device>> GetDevices();
        Task Add(Device device);
        Task<bool> IsValidNewDevice(Device device);
    }
}