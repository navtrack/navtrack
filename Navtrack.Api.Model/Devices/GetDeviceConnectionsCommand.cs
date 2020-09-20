namespace Navtrack.Api.Model.Devices
{
    public class GetDeviceConnectionsCommand 
    {
        public int DeviceId { get; set; }
        public int UserId { get; set; }
        public int Page { get; set; }
        public int PerPage { get; set; }
    }
}