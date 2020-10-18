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
    [Service(typeof(ICommandHandler<GetTripsCommand, GetTripsResponseModel>))]
    public class GetTripsCommandHandler : BaseCommandHandler<GetTripsCommand, GetTripsResponseModel>
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
            if (!await assetDataService.UserHasRolesForAsset(command.UserId,
                new[] {UserAssetRole.User, UserAssetRole.Owner}, command.AssetId))
            {
                ApiResponse.IsUnauthorised();
            }
        }

        public override async Task<GetTripsResponseModel> Handle(GetTripsCommand command)
        {
            await tripDataService.UpdateTrips(command.AssetId);

            IEnumerable<TripResponseModel> trips =
                (await tripDataService.GetTrips(command.AssetId,
                    mapper.Map<LocationFilterRequestModel, LocationFilter>(command.Model)))
                .Select(x =>
                    new TripResponseModel
                    {
                        Number = x.Trip.Number,
                        Distance = x.Trip.Distance,
                        Locations = x.Locations.Select(y => mapper.Map<LocationEntity, LocationResponseModel>(y))
                            .ToList()
                    }).ToList();

            return new GetTripsResponseModel
            {
                Results = trips.OrderByDescending(x => x.EndDate).ToList()
            };
        }
    }
}