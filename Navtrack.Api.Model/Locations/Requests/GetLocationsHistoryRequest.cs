namespace Navtrack.Api.Model.Locations.Requests
{
    public class GetLocationsHistoryRequest : BaseRequest<LocationHistoryRequestModel>
    {
        public int UserId { get; set; }
    }
}