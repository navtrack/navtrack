using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Navtrack.Web.Model.Models;

namespace Navtrack.Web.Services
{
    public interface IDeviceService
    {
        Task<List<DeviceModel>> Get();
        Task Add(DeviceModel deviceModel);
        Task<bool> IsValidNewDevice(DeviceModel model);
        Task<DeviceModel> Get(int id);
        List<ProtocolModel> GetProtocols();
        Task Update(DeviceModel model);
    }
}