namespace Navtrack.Api.Model.Devices
{
    public class GetDeviceByIdCommand 
    {
        public int DeviceId { get; set; }
        public int UserId { get; set; }
    }
}