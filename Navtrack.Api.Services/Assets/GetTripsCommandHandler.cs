using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Navtrack.Api.Model.Assets;
using Navtrack.Api.Services.CommandHandler;
using Navtrack.DataAccess.Model;
using Navtrack.DataAccess.Model.Custom;
using Navtrack.DataAccess.Services;
using Navtrack.Library.DI;
using Navtrack.Library.Services;

namespace Navtrack.Api.Services.Assets
{
    [Service(typeof(ICommandHandler<GetTripsCommand, GetTripsModel>))]
    public class GetTripsCommandHandler : BaseCommandHandler<GetTripsCommand, GetTripsModel>
    {
        private readonly IAssetDataService assetDataService;
        private readonly IMapper mapper;
        private readonly ITripDataService tripDataService;

        public GetTripsCommandHandler(IAssetDataService assetDataService, IMapper mapper,
            ITripDataService tripDataService)
        {
            this.assetDataService = assetDataService;
            this.mapper = mapper;
            this.tripDataService = tripDataService;
        }

        public override async Task Authorize(GetTripsCommand command)
        {
            if (!await assetDataService.UserHasRoleForAsset(command.UserId, UserAssetRole.Owner, command.AssetId))
            {
                ApiResponse.IsUnauthorised();
            }
        }

        public override async Task<GetTripsModel> Handle(GetTripsCommand command)
        {
            await tripDataService.UpdateTrips(command.AssetId);
            
            IEnumerable<TripModel> trips =
                (await tripDataService.GetTrips(command.AssetId)).Select(x => new TripModel
                {
                    Number = x.Trip.Number,
                    Distance = x.Trip.Distance,
                    Locations = x.Locations.Select(y => mapper.Map<LocationEntity, LocationModel>(y)).ToList()
                }).ToList();
            
            return new GetTripsModel
            {
                Results = trips.OrderByDescending(x => x.EndDate).ToList()
            };
        }
    }
}