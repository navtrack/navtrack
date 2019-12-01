using System.Collections.Generic;

namespace Navtrack.DataAccess.Model
{
    public class Device : IEntity
    {
        public Device()
        {
            Locations = new HashSet<Location>();
        }
        
        public int Id { get; set; }
        public string IMEI { get; set; }
        public string Name { get; set; }
        public Asset Asset { get; set; }
        public int DeviceTypeId { get; set; }
        public DeviceType DeviceType { get; set; }
        public ICollection<Location> Locations { get; set; }
    }
}