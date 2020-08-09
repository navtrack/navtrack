using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Navtrack.Api.Model.Locations;
using Navtrack.Api.Model.Locations.Requests;
using Navtrack.Api.Services.RequestHandlers;
using Navtrack.DataAccess.Model;
using Navtrack.DataAccess.Repository;
using Navtrack.Library.DI;
using Navtrack.Library.Services;

namespace Navtrack.Api.Services.Locations
{
    [Service(typeof(IRequestHandler<GetLocationsHistoryRequest, IEnumerable<LocationResponseModel>>))]
    public class GetLocationsHistoryRequestHandler : BaseRequestHandler<GetLocationsHistoryRequest, IEnumerable<LocationResponseModel>>
    {
        private readonly IRepository repository;
        private readonly IMapper mapper;

        public GetLocationsHistoryRequestHandler(IRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public override async Task<IEnumerable<LocationResponseModel>> Handle(GetLocationsHistoryRequest request)
        {
            IQueryable<LocationEntity> queryable = repository.GetEntities<LocationEntity>();

            queryable = ApplyFiltering(queryable, request.Body);

            queryable = queryable
                .OrderByDescending(x => x.DateTime)
                .Take(100000);


            List<LocationEntity> locations = await queryable.ToListAsync();

            List<LocationResponseModel> mapped = locations.Select(mapper.Map<LocationEntity, LocationResponseModel>).ToList();

            return mapped;
        }
        
        private static IQueryable<LocationEntity> ApplyFiltering(IQueryable<LocationEntity> queryable,
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