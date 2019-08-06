using System.Collections.Generic;
using System.Threading.Tasks;
using Navtrack.DataAccess.Model;

namespace Navtrack.Common.DataServices
{
    public interface ILocationDataService
    {
        Task Add(Location location);
        Task AddRange(IEnumerable<Location> locations);
    }
}