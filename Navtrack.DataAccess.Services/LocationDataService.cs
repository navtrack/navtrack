using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Navtrack.DataAccess.Model;
using Navtrack.DataAccess.Model.Custom;
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

        public IQueryable<LocationEntity> GetLocations(int assetId, LocationFilter locationFilter)
        {
            IQueryable<LocationEntity> queryable = repository.GetEntities<LocationEntity>()
                .OrderByDescending(x => x.DateTime)
                .Where(x => x.AssetId == assetId);

            queryable = ApplyFiltering(queryable, locationFilter);

            return queryable;
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
        
        
        private static IQueryable<LocationEntity> ApplyFiltering(IQueryable<LocationEntity> queryable,
            LocationFilter locationFilter)
        {
            if (locationFilter.StartDate.HasValue)
            {
                queryable = queryable.Where(x => x.DateTime >= locationFilter.StartDate.Value);
            }

            if (locationFilter.EndDate.HasValue)
            {
                queryable = queryable.Where(x => x.DateTime <= locationFilter.EndDate.Value);
            }

            if (locationFilter.MinSpeed.HasValue)
            {
                queryable = queryable.Where(x => x.Speed >= locationFilter.MinSpeed.Value);
            }

            if (locationFilter.MaxSpeed.HasValue)
            {
                queryable = queryable.Where(x => x.Speed <= locationFilter.MaxSpeed.Value);
            }

            if (locationFilter.MinAltitude.HasValue)
            {
                queryable = queryable.Where(x => x.Altitude >= locationFilter.MinAltitude.Value);
            }

            if (locationFilter.MaxAltitude.HasValue)
            {
                queryable = queryable.Where(x => x.Altitude <= locationFilter.MaxAltitude.Value);
            }

            return queryable;
        }
    }
}