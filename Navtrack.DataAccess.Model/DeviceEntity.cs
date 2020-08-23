using System.Collections.Generic;
using Navtrack.DataAccess.Model.Common;

namespace Navtrack.DataAccess.Model
{
    public class DeviceEntity : EntityAudit, IEntity
    {
        public DeviceEntity()
        {
            Locations = new HashSet<LocationEntity>();
            Connections = new HashSet<DeviceConnectionEntity>();
        }
        
        public int Id { get; set; }
        public string DeviceId { get; set; }
        public int ProtocolPort { get; set; }
        public int AssetId { get; set; }
        public bool IsActive { get; set; }
        public AssetEntity Asset { get; set; }
        public int DeviceTypeId { get; set; }
        public ICollection<LocationEntity> Locations { get; set; }
        public ICollection<DeviceConnectionEntity> Connections { get; set; }
    }
}