using System.Collections.Generic;
using System.Threading.Tasks;
using Navtrack.Web.Models;

namespace Navtrack.Web.Services
{
    public interface ILocationService
    {
        Task<LocationModel> GetLatestLocation(int assetId);
        Task<List<LocationModel>> GetLocations(int assetId);
    }
}