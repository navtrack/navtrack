using Navtrack.Api.Model.Custom;

namespace Navtrack.Api.Model.Devices
{
    public class DeviceModel : IModel
    {
        public int Id { get; set; }
        public int AssetId { get; set; }
        public string DeviceId { get; set; }
        public DeviceTypeModel DeviceType { get; set; }
        public int LocationsCount { get; set; }
        public bool IsActive { get; set; }
    }
}