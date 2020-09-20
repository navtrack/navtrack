using Navtrack.Api.Model.Protocols;

namespace Navtrack.Api.Model.Devices
{
    public class DeviceTypeModel
    {
        public int Id { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public string DisplayName { get; set; }
        public ProtocolModel Protocol { get; set; }
    }
}