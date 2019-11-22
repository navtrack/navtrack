using System.Threading.Tasks;
using Navtrack.DataAccess.Model;

namespace Navtrack.Common.DataServices
{
    public interface IObjectDataService
    {
        Task<Asset> GetObjectByIMEI(string imei);
    }
}