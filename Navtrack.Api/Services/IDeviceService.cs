using System.Collections.Generic;
using System.Threading.Tasks;
using Navtrack.Api.Models;
using Navtrack.Api.Services.Generic;
using Navtrack.DataAccess.Model;

namespace Navtrack.Api.Services
{
    public interface IDeviceService : IGenericService<DeviceEntity, DeviceModel>
    {
        Task<List<DeviceModel>> GetAllAvailableForAsset(int? assetId);
    }
}