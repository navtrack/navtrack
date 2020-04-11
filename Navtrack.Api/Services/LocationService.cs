using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Navtrack.Api.Models;
using Navtrack.Api.Models.Locations;
using Navtrack.DataAccess.Model;
using Navtrack.DataAccess.Repository;
using Navtrack.Library.DI;
using Navtrack.Library.Services;

namespace Navtrack.Api.Services
{
    [Service(typeof(ILocationService))]
    public class LocationService : ILocationService
    {
        private readonly IRepository repository;
        private readonly IMapper mapper;

        public LocationService(IRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<LocationModel> GetLatestLocation(int assetId)
        {
            Location location = await repository.GetEntities<Location>()
                .Where(x => x.AssetId == assetId)
                .OrderByDescending(x => x.DateTime)
                .FirstOrDefaultAsync();

            if (location != null)
            {
                LocationModel mapped = mapper.Map<Location, LocationModel>(location);

                return mapped;
            }

            return null;
        }

        public async Task<List<LocationModel>> GetLocations(int assetId)
        {
            List<Location> locations = await repository.GetEntities<Location>()
                .Where(x => x.AssetId == assetId)
                .OrderByDescending(x => x.DateTime)
                .Take(1000)
                .ToListAsync();


            List<LocationModel> mapped = locations.Select(mapper.Map<Location, LocationModel>).ToList();

            return mapped;
        }

        public async Task<List<LocationModel>> GetLocations(LocationHistoryRequestModel model)
        {
            IQueryable<Location> queryable = repository.GetEntities<Location>();

            queryable = ApplyFiltering(queryable, model);

            queryable = queryable
                .OrderByDescending(x => x.DateTime)
                .Take(100000);


            List<Location> locations = await queryable.ToListAsync();

            List<LocationModel> mapped = locations.Select(mapper.Map<Location, LocationModel>).ToList();

            return mapped;
        }

        private static IQueryable<Location> ApplyFiltering(IQueryable<Location> queryable,
            LocationHistoryRequestModel model)
        {
            queryable = queryable.Where(x => x.AssetId == model.AssetId);

            queryable = queryable.Where(x => x.DateTime > model.StartDate && x.DateTime < model.EndDate);

            if (model.Latitude.HasValue && model.Longitude.HasValue && model.Radius.HasValue)
            {
                // TODO add location filter   
            }

            if (model.StartSpeed.HasValue && model.EndSpeed.HasValue)
            {
                queryable = queryable.Where(x => x.Speed >= model.StartSpeed && x.Speed <= model.EndSpeed);
            }
            else if (model.StartSpeed.HasValue)
            {
                queryable = queryable.Where(x => x.Speed >= model.StartSpeed);
            }
            else if (model.EndSpeed.HasValue)
            {
                queryable = queryable.Where(x => x.Speed <= model.EndSpeed);
            }
            
            if (model.StartAltitude.HasValue && model.EndAltitude.HasValue)
            {
                queryable = queryable.Where(x => x.Altitude >= model.StartAltitude && x.Altitude <= model.EndAltitude);
            }
            else if (model.StartAltitude.HasValue)
            {
                queryable = queryable.Where(x => x.Altitude >= model.StartAltitude);
            }
            else if (model.EndAltitude.HasValue)
            {
                queryable = queryable.Where(x => x.Altitude <= model.EndAltitude);
            }
            
            return queryable;
        }
    }
}