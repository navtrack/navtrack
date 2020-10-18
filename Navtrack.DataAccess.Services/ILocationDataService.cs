using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Navtrack.DataAccess.Model;
using Navtrack.DataAccess.Model.Custom;

namespace Navtrack.DataAccess.Services
{
    public interface ILocationDataService
    {
        Task Add(LocationEntity location);
        Task AddRange(IEnumerable<LocationEntity> locations);
        IQueryable<LocationEntity> GetLocations(int assetId, LocationFilter locationFilter);
    }
}