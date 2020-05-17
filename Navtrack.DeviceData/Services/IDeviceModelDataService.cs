using System.Threading.Tasks;
using Navtrack.DeviceData.Model;

namespace Navtrack.DeviceData.Services
{
    public interface IDeviceModelDataService
    {
        DeviceModel GetById(int id);
        DeviceModel[] GetDeviceModels();
    }
}