using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Navtrack.Api.Model.Assets;
using Navtrack.Api.Services.RequestHandlers;
using Navtrack.DataAccess.Model;
using Navtrack.DataAccess.Repository;
using Navtrack.Library.DI;
using Navtrack.Library.Services;

namespace Navtrack.Api.Services.Locations
{
    [Service(typeof(ICommandHandler<GetLocationsCommand, IEnumerable<LocationModel>>))]
    public class GetLocationsHistoryCommandHandler : BaseCommandHandler<GetLocationsCommand, IEnumerable<LocationModel>>
    {
        private readonly IRepository repository;
        private readonly IMapper mapper;

        public GetLocationsHistoryCommandHandler(IRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public override async Task<IEnumerable<LocationModel>> Handle(GetLocationsCommand command)
        {
            IQueryable<LocationEntity> queryable = repository.GetEntities<LocationEntity>();

            queryable = ApplyFiltering(queryable, command);

            queryable = queryable
                .OrderByDescending(x => x.DateTime)
                .Take(100000);


            List<LocationEntity> locations = await queryable.ToListAsync();

            List<LocationModel> mapped = locations.Select(mapper.Map<LocationEntity, LocationModel>).ToList();

            return mapped;
        }
        
        private static IQueryable<LocationEntity> ApplyFiltering(IQueryable<LocationEntity> queryable,
            GetLocationsCommand command)
        {
            queryable = queryable.Where(x => x.AssetId == command.AssetId);

            queryable = queryable.Where(x => x.DateTime > command.Model.StartDate && x.DateTime < command.Model.EndDate);

            if (command.Model.StartSpeed.HasValue && command.Model.EndSpeed.HasValue)
            {
                queryable = queryable.Where(x => x.Speed >= command.Model.StartSpeed && x.Speed <= command.Model.EndSpeed);
            }
            else if (command.Model.StartSpeed.HasValue)
            {
                queryable = queryable.Where(x => x.Speed >= command.Model.StartSpeed);
            }
            else if (command.Model.EndSpeed.HasValue)
            {
                queryable = queryable.Where(x => x.Speed <= command.Model.EndSpeed);
            }
            
            if (command.Model.StartAltitude.HasValue && command.Model.EndAltitude.HasValue)
            {
                queryable = queryable.Where(x => x.Altitude >= command.Model.StartAltitude && x.Altitude <= command.Model.EndAltitude);
            }
            else if (command.Model.StartAltitude.HasValue)
            {
                queryable = queryable.Where(x => x.Altitude >= command.Model.StartAltitude);
            }
            else if (command.Model.EndAltitude.HasValue)
            {
                queryable = queryable.Where(x => x.Altitude <= command.Model.EndAltitude);
            }
            
            return queryable;
        }
    }
}