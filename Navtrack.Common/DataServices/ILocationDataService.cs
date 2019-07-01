using System.Threading.Tasks;
using Navtrack.DataAccess.Model;

namespace Navtrack.Common.DataServices
{
    public interface ILocationDataService
    {
        Task Add(Location location);
    }
}