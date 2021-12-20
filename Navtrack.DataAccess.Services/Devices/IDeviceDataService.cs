using System.Threading.Tasks;
using Navtrack.DataAccess.Model.Assets;

namespace Navtrack.DataAccess.Services.Devices;

public interface IDeviceDataService
{
    Task<bool> SerialNumberIsUsed(string serialNumber, int protocolPort, string excludeAssetId);
    Task<AssetDocument> GetActiveDeviceByDeviceId(string deviceId);
}