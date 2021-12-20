using System.Threading.Tasks;
using Navtrack.Api.Model.Locations;

namespace Navtrack.Api.Services.Locations;

public interface ILocationService
{
    Task<LocationListModel> GetLocations(string assetId, LocationFilterModel locationFilter, int page, int size);
}