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

namespace Navtrack.Api.Services.Assets
{
    [Service(typeof(IRequestHandler<GetAllAssetsCommand, IEnumerable<AssetModel>>))]
    public class GetAllAssetsCommandHandler : BaseRequestHandler<GetAllAssetsCommand, IEnumerable<AssetModel>>
    {
        private readonly IMapper mapper;
        private readonly IRepository repository;

        public GetAllAssetsCommandHandler(IMapper mapper, IRepository repository)
        {
            this.mapper = mapper;
            this.repository = repository;
        }

        public override async Task<IEnumerable<AssetModel>> Handle(GetAllAssetsCommand command)
        {
            var entities = await (from asset in repository.GetEntities<AssetEntity>()
                        .Where(x => x.Users.Any(y => y.UserId == command.UserId))
                    join latestLocation in repository.GetEntities<LocationEntity>()
                            .GroupBy(x => x.AssetId)
                            .Select(x => new {AssetId = x.Key, LatestDateTime = x.Max(y => y.DateTime)})
                        on asset.Id equals latestLocation.AssetId into latestLocations
                    from latestLocation in latestLocations.DefaultIfEmpty()
                    join location in repository.GetEntities<LocationEntity>()
                        on new {AssetId = asset.Id, DateTime = latestLocation.LatestDateTime} equals new
                            {location.AssetId, location.DateTime}
                        into locations
                    from location in locations.DefaultIfEmpty()
                    select new {asset, location})
                .ToListAsync();

            List<AssetModel> models = entities
                .Select(x => mapper.Map<AssetEntity, LocationEntity, AssetModel>(x.asset, x.location))
                .ToList();

            return models;
        }
    }
}