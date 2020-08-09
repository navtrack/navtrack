using System.Collections.Generic;
using Navtrack.DataAccess.Model.Common;

namespace Navtrack.DataAccess.Model
{
    public class AssetEntity : EntityAudit, IEntity
    {
        public AssetEntity()
        {
            Locations = new HashSet<LocationEntity>();
            Users = new HashSet<UserAssetEntity>();
            Devices = new HashSet<DeviceEntity>();
        }
        
        public int Id { get; set; }
        public string Name { get; set; }
        public int? DeviceId { get; set; }
        public ICollection<DeviceEntity> Devices { get; set; }
        public ICollection<LocationEntity> Locations { get; set; }
        public ICollection<UserAssetEntity> Users { get; set; }
    }
}