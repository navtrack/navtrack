using System;

namespace Navtrack.Api.Model.Devices
{
    public class DeviceConnectionResponseModel : IModel
    {
        public int Id { get; set; }
        public int DeviceId { get; set; }
        public DateTime OpenedAt { get; set; }
        public DateTime? ClosedAt { get; set; }
        public string RemoteEndPoint { get; set; }
    }
}