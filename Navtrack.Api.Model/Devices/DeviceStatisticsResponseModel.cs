using System;

namespace Navtrack.Api.Model.Devices
{
    public class DeviceStatisticsResponseModel
    {
        public int Locations { get; set; }
        public DateTime? FirstLocationDateTime { get; set; }
        public DateTime? LastLocationDateTime { get; set; }
        public int Connections { get; set; }
    }
}