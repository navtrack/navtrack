using System.Collections.Generic;
using Navtrack.DataAccess.Model.Common;

namespace Navtrack.DataAccess.Model
{
    public class DeviceEntity : EntityAudit, IEntity
    {
        public DeviceEntity()
        {
            Locations = new HashSet<LocationEntity>();
            Users = new HashSet<UserDeviceEntity>();
        }
        
        public int Id { get; set; }
        public string DeviceId { get; set; }
        public string Name { get; set; }
        public AssetEntity Asset { get; set; }
        public int DeviceModelId { get; set; }
        public ICollection<LocationEntity> Locations { get; set; }
        public ICollection<UserDeviceEntity> Users { get; set; }
    }
}