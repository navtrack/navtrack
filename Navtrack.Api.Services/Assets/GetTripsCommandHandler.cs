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

        private IEnumerable<TripModel> MapTrips(IEnumerable<LocationEntity> locations,
            int medianTimeSpanBetweenLocations)
        {
            List<TripModel> trips = new List<TripModel>();

            TripModel lastTrip = null;

            int tripNumber = 1;

            foreach (LocationEntity location in locations)
            {
                TimeSpan? timeSpan = GetTimeSpan(lastTrip, location);
                
                // TODO add condition when it's the same connection id 
                if (lastTrip == null || timeSpan == null || timeSpan.Value.TotalSeconds > medianTimeSpanBetweenLocations)
                {
                    lastTrip = new TripModel
                    {
                        Number = tripNumber++
                    };
                    trips.Add(lastTrip);
                }

                lastTrip.Locations.Add(mapper.Map<LocationEntity, LocationModel>(location));
            }
            
            return trips;
        }

        private static TimeSpan? GetTimeSpan(TripModel lastTrip, LocationEntity nextLocation)
        {
            LocationModel lastLocation = lastTrip?.Locations.LastOrDefault();

            if (lastLocation != null)
            {
                TimeSpan timeSpan = nextLocation.DateTime - lastLocation.DateTime;

                return timeSpan;
            }

            return null;
        }
    }
}