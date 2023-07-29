using System.Collections.Generic;
using System.Threading.Tasks;
using Navtrack.Api.Model.Trips;
using Navtrack.Api.Services.Mappers.Trips;
using Navtrack.Api.Services.User;
using Navtrack.Core.Model.Trips;
using Navtrack.DataAccess.Model.Users;
using Navtrack.Library.DI;

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
        UserDocument user = await currentUserAccessor.Get();

        TripListModel list = TripListMapper.Map(trips);
            
        return list;
    }
}