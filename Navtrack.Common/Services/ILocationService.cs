using System.Threading.Tasks;
using Navtrack.Common.Model;

namespace Navtrack.Common.Services
{
    public interface ILocationService
    {
        Task Add(Location location);
    }
}