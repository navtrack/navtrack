using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Navtrack.Api.Model.Assets;
using Navtrack.Api.Services.RequestHandlers;
using Navtrack.DataAccess.Model;
using Navtrack.DataAccess.Model.Custom;
using Navtrack.DataAccess.Repository;
using Navtrack.DataAccess.Services;
using Navtrack.Library.DI;
using Navtrack.Library.Services;

namespace Navtrack.Api.Services.Assets
{
    [Service(typeof(IRequestHandler<GetTripsCommand, GetTripsModel>))]
    public class GetTripsCommandHandler : BaseRequestHandler<GetTripsCommand, GetTripsModel>
    {
        private readonly IRepository repository;
        private readonly IAssetDataService assetDataService;
        private readonly IMapper mapper;
        private readonly ITripDataService tripDataService;

        public GetTripsCommandHandler(IRepository repository, IAssetDataService assetDataService, IMapper mapper,
            ITripDataService tripDataService)
        {
            this.repository = repository;
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
            DateTime lastMonth = DateTime.UtcNow.AddMonths(-1);

            List<LocationEntity> locations = await repository.GetEntities<LocationEntity>()
                .Where(x => x.AssetId == command.AssetId && x.DateTime > lastMonth)
                .OrderBy(x => x.DateTime)
                .ToListAsync();

            int medianTimeSpanBetweenLocations =
                await tripDataService.GetMedianTimeSpanBetweenLocations(command.AssetId);

            IEnumerable<TripModel> trips = MapTrips(locations, medianTimeSpanBetweenLocations);

            return new GetTripsModel
            {
                Results = trips.OrderByDescending(x => x.EndDate).ToList()
            };
        }

        private IEnumerable<TripModel> MapTrips(IReadOnlyList<LocationEntity> locations,
            int medianTimeSpanBetweenLocations)
        {
            List<TripModel> trips = new List<TripModel>();

            TripModel trip = null;
            int tripNo = 1;

            for (int i = 0; i < locations.Count - 1; i++)
            {
                TimeSpan nextTimeSpan = locations[i + 1].DateTime - locations[i].DateTime;

                if (nextTimeSpan.TotalSeconds > medianTimeSpanBetweenLocations || trip == null)
                {
                    trip = new TripModel
                    {
                        Number = tripNo++
                    };

                    trips.Add(trip);
                }

                trip.Locations.Add(mapper.Map<LocationEntity, LocationModel>(locations[i]));
            }

            return trips;
        }
    }
}