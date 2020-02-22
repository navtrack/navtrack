using System.Collections.Generic;
using System.Threading.Tasks;
using Navtrack.Web.Models;
using Navtrack.Web.Models.Locations;

namespace Navtrack.Web.Services
{
    public interface ILocationService
    {
        Task<LocationModel> GetLatestLocation(int assetId);
        Task<List<LocationModel>> GetLocations(int assetId);
        Task<List<LocationModel>> GetLocations(LocationHistoryRequestModel model);
    }
}