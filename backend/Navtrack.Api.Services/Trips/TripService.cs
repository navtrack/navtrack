using System.Collections.Generic;
using System.Threading.Tasks;
using Navtrack.Api.Model.Trips;
using Navtrack.Api.Services.Mappers;
using Navtrack.Api.Services.User;
using Navtrack.DataAccess.Model.Users;
using Navtrack.Library.DI;
using Trip = Navtrack.Core.Model.Trips.Trip;

namespace Navtrack.Api.Services.Trips;

[Service(typeof(ITripService))]
public class TripService : ITripService
{
    private readonly Core.Services.Trips.ITripService tripService;
    private readonly ICurrentUserAccessor currentUserAccessor;

    public TripService(Core.Services.Trips.ITripService tripService, ICurrentUserAccessor currentUserAccessor)
    {
        this.tripService = tripService;
        this.currentUserAccessor = currentUserAccessor;
    }

    public async Task<TripListModel> GetTrips(string assetId, TripFilterModel tripFilter)
    {
        IEnumerable<Trip> trips = await tripService.GetTrips(assetId, tripFilter);
        UserDocument user = await currentUserAccessor.GetCurrentUser();

        TripListModel list = TripListMapper.Map(trips, user.UnitsType);
            
        return list;
    }
}