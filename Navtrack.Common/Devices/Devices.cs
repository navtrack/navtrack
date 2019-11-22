using System.Collections.Generic;

namespace Navtrack.Common.Devices
{
    public static class Devices 
    {
        public static List<DeviceModel> List = new List<DeviceModel>
        {
            new DeviceModel("Teltonika", "FM3200", Protocol.Teltonika),
            new DeviceModel("Teltonika", "FM4200", Protocol.Teltonika),
            new DeviceModel("Meitrack", "One", Protocol.Meitrack),
            new DeviceModel("Meitrack", "Two", Protocol.Meitrack)
        };
    }
}