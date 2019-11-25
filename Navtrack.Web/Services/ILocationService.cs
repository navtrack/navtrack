using System.Threading.Tasks;
using Navtrack.Web.Models;

namespace Navtrack.Web.Services
{
    public interface ILocationService
    {
        Task<LocationModel> GetLatestLocation(int assetId);
    }
}