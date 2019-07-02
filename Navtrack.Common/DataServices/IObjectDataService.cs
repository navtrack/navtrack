using System.Threading.Tasks;
using Navtrack.DataAccess.Model;

namespace Navtrack.Common.DataServices
{
    public interface IObjectDataService
    {
        Task<Object> GetObjectByIMEI(string imei);
    }
}