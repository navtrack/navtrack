using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Navtrack.Api.Model.Assets;
using Navtrack.Api.Services.CommandHandler;
using Navtrack.DataAccess.Model;
using Navtrack.DataAccess.Model.Custom;
using Navtrack.DataAccess.Services;
using Navtrack.Library.DI;
using Navtrack.Library.Services;

namespace Navtrack.Api.Services.Assets
{
    [Service(typeof(ICommandHandler<GetLocationsCommand, GetLocationsResponseModel>))]
    public class GetLocationsCommandHandler : BaseCommandHandler<GetLocationsCommand, GetLocationsResponseModel>
    {
        private readonly IMapper mapper;
        private readonly IAssetDataService assetDataService;
        private readonly ILocationDataService locationDataService;

        public GetLocationsCommandHandler(IMapper mapper, IAssetDataService assetDataService,
            ILocationDataService locationDataService)
        {
            this.mapper = mapper;
            this.assetDataService = assetDataService;
            this.locationDataService = locationDataService;
        }

        public override async Task Authorize(GetLocationsCommand command)
        {
            if (!await assetDataService.UserHasRolesForAsset(command.UserId,
                new[] {UserAssetRole.User, UserAssetRole.Owner}, command.AssetId))
            {
                ApiResponse.IsUnauthorised();
            }
        }

        public override async Task<GetLocationsResponseModel> Handle(GetLocationsCommand command)
        {
            IQueryable<LocationEntity> queryable = locationDataService.GetLocations(command.AssetId,
                mapper.Map<LocationFilterRequestModel, LocationFilter>(command.Model));

            int totalResults = await queryable.CountAsync();

            queryable = ApplyLimit(queryable, command.Model);

            List<LocationEntity> locations = await queryable.ToListAsync();

            List<LocationResponseModel> mapped = locations.Select(mapper.Map<LocationEntity, LocationResponseModel>)
                .ToList();

            return new GetLocationsResponseModel
            {
                Results = mapped,
                TotalResults = totalResults
            };
        }

        private static IQueryable<LocationEntity> ApplyLimit(IQueryable<LocationEntity> queryable,
            LocationFilterRequestModel model)
        {
            int limit = model.HasFilters ? 1000 : 500;

            queryable = queryable.Take(limit);

            return queryable;
        }
    }
}