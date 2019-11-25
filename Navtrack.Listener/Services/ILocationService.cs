using System.Collections.Generic;
using System.Threading.Tasks;
using Navtrack.Listener.Models;

namespace Navtrack.Listener.Services
{
    public interface ILocationService
    {
        Task Add(Location location);
        Task AddRange(List<Location> locations);
    }
}