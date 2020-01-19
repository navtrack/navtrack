using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Navtrack.DataAccess.Model;
using Navtrack.DataAccess.Repository;
using Navtrack.Library.DI;
using Navtrack.Library.Services;
using Navtrack.Web.Models;

namespace Navtrack.Web.Services
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
    }
}