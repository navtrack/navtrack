using System.Collections.Generic;
using System.Threading.Tasks;
using Navtrack.Web.Models;

namespace Navtrack.Web.Services
{
    public interface IDeviceService
    {
        Task<List<Device>> GetDevices();
    }
}