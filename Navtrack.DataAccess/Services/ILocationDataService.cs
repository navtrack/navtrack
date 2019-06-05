using System.Threading.Tasks;
using Navtrack.DataAccess.Model;

namespace Navtrack.DataAccess.Services
{
    public interface ILocationDataService
    {
        Task Add(Location location);
    }
}