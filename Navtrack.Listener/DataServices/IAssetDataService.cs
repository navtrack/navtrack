using System.Threading.Tasks;
using Navtrack.DataAccess.Model;

namespace Navtrack.Listener.DataServices
{
    public interface IAssetDataService
    {
        Task<AssetEntity> GetAssetByIMEI(string imei);
    }
}