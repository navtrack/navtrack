using Navtrack.Database.Model.Protocols;

namespace Navtrack.Database.Model.Devices;

public class DeviceType
{
    public string Id { get; set; }
    public string Manufacturer { get; set; }
    public string Type { get; set; }
    public Protocol Protocol { get; set; }
}