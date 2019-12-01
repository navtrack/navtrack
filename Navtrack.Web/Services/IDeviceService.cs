using System.Collections.Generic;
using System.Threading.Tasks;
using Navtrack.DataAccess.Model;
using Navtrack.Web.Models;
using Navtrack.Web.Services.Generic;

namespace Navtrack.Web.Services
{
    public interface IDeviceService : IGenericService<Device, DeviceModel>
    {
        IEnumerable<ProtocolModel> GetProtocols();
        Task<List<DeviceTypeModel>> GetTypes();
        Task<List<DeviceModel>> GetAllAvailableIncluding(int? id);
    }
}