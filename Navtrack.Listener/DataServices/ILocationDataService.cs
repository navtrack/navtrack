using System.Collections.Generic;
using System.Threading.Tasks;
using Navtrack.DataAccess.Model;

namespace Navtrack.Listener.DataServices
{
    public interface ILocationDataService
    {
        Task Add(LocationEntity location);
        Task AddRange(IEnumerable<LocationEntity> locations);
    }
}