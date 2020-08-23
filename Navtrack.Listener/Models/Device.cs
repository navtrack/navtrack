using Navtrack.DataAccess.Model;

namespace Navtrack.Listener.Models
{
    public class Device
    {
        public string IMEI { get; set; }
        public DeviceEntity Entity { get; set; }
    }
}