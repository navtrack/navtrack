using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Navtrack.DataAccess.Model;
using Navtrack.DataAccess.Repository;
using Navtrack.Library.DI;

namespace Navtrack.DataAccess.Services
{
    [Service(typeof(ILocationDataService))]
    public class LocationDataService : ILocationDataService
    {
        private readonly IRepository repository;

        public LocationDataService(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task Add(LocationEntity location)
        {
            using IUnitOfWork unitOfWork = repository.CreateUnitOfWork();
            
            unitOfWork.Add(location);

            await unitOfWork.SaveChanges();
        }

        public async Task AddRange(IEnumerable<LocationEntity> locations)
        {
            using IUnitOfWork unitOfWork = repository.CreateUnitOfWork();
            
            unitOfWork.AddRange(locations);

            await unitOfWork.SaveChanges();
        }

        public async Task<int> GetMedianLocationTimeSpan(int assetId)
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