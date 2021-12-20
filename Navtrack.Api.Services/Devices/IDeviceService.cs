using System.Threading.Tasks;

namespace Navtrack.Api.Services.Devices;

public interface IDeviceService
{
    Task<bool> SerialNumberIsUsed(string serialNumber, string deviceTypeId, string excludeAssetId = null);
}