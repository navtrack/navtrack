using System.Collections.Generic;
using System.Threading.Tasks;
using Navtrack.Api.Models;
using Navtrack.Api.Models.Locations;

namespace Navtrack.Api.Services
{
    public interface ILocationService
    {
        Task<LocationModel> GetLatestLocation(int assetId);
        Task<List<LocationModel>> GetLocations(int assetId);
        Task<List<LocationModel>> GetLocations(LocationHistoryRequestModel model);
    }
}