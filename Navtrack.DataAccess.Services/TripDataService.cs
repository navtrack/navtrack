using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Navtrack.DataAccess.Model;
using Navtrack.DataAccess.Repository;
using Navtrack.Library.DI;

namespace Navtrack.DataAccess.Services
{
    [Service(typeof(ITripDataService))]
    public class TripDataService : ITripDataService
    {
        private readonly IRepository repository;

        public TripDataService(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task<int> GetMedianTimeSpanBetweenLocations(int assetId)
        {
            List<LocationEntity> locations = await repository.GetEntities<LocationEntity>()
                .Where(x => x.AssetId == assetId)
                .OrderByDescending(x => x.DateTime)
                .Take(100)
                .ToListAsync();

            List<double> seconds = new List<double>();
            
            for (int i = 1; i < locations.Count; i++)
            {
                seconds.Add((locations[i-1].DateTime - locations[i].DateTime).TotalSeconds);
            }

            seconds = seconds.OrderBy(x => x).ToList();

            return (int) seconds[seconds.Count / 2]*2;
        }
    }
}