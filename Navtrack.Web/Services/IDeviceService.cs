using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Navtrack.Web.Models;

namespace Navtrack.Web.Services
{
    public interface IDeviceService
    {
        Task<List<DeviceModel>> GetAll();
        Task Add(DeviceModel device);
        Task<DeviceModel> Get(int id);
        IEnumerable<ProtocolModel> GetProtocols();
        Task Update(DeviceModel device);
        Task<bool> IMEIAlreadyExists(string imei);
        Task<List<DeviceTypeModel>> GetTypes();
        Task ValidateModel(DeviceModel device, ModelStateDictionary modelState);
        Task<List<DeviceModel>> GetAllAvailableIncluding(int? id);
    }
}