using System.Collections.Generic;
using System.Threading.Tasks;
using Navtrack.Web.Model.Models;

namespace Navtrack.Web.Services
{
    public interface IDeviceService
    {
        Task<List<DeviceModel>> GetAll();
        Task Add(DeviceModel deviceModel);
        Task<DeviceModel> Get(int id);
        IEnumerable<ProtocolModel> GetProtocols();
        Task Update(DeviceModel model);
        Task<bool> IMEIAlreadyExists(string imei);
    }
}