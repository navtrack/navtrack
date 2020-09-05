namespace Navtrack.Api.Model.Devices.Requests
{
    public class GetDeviceConnectionsRequest 
    {
        public int DeviceId { get; set; }
        public int UserId { get; set; }
        public int Page { get; set; }
        public int PerPage { get; set; }
    }
}