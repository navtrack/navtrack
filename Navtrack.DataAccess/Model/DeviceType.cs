using System.Collections.Generic;

namespace Navtrack.DataAccess.Model
{
    public class DeviceType
    {
        public DeviceType()
        {
            Devices = new HashSet<Device>();
        }
        
        public int Id { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int ProtocolId { get; set; }
        public ICollection<Device> Devices { get; set; }
    }
}