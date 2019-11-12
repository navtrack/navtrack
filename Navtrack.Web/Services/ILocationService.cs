using System.Collections.Generic;
using System.Threading.Tasks;
using Navtrack.Web.Model;

namespace Navtrack.Web.Services
{
    public interface ILocationService
    {
        Task<List<LocationModel>> Get();
        Task<LocationModel> GetLatestLocation(int objectId);
    }
}