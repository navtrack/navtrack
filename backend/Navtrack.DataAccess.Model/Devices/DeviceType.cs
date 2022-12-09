using Navtrack.DataAccess.Model.Protocols;

namespace Navtrack.DataAccess.Model.Devices;

public class DeviceType
{
    public string Id { get; set; }
    public string Manufacturer { get; set; }
    public string Type { get; set; }
    public Protocol Protocol { get; set; }
}