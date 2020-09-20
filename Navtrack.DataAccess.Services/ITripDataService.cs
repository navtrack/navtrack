using System.Threading.Tasks;

namespace Navtrack.DataAccess.Services
{
    public interface ITripDataService
    {
        Task<int> GetMedianTimeSpanBetweenLocations(int assetId);
    }
}