namespace Navtrack.Api.Model.Devices
{
    public class DeviceModel : IModel
    {
        public int Id { get; set; }
        public string DeviceId { get; set; }
        public DeviceTypeResponseModel DeviceType { get; set; }
        public int LocationsCount { get; set; }
        public bool IsActive { get; set; }
    }
}