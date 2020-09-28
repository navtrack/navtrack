using System.Collections.Generic;
using Navtrack.DataAccess.Model.Common;

namespace Navtrack.DataAccess.Model
{
    public class UserEntity : EntityAudit, IEntity
    {
        public UserEntity()
        {
            Assets = new HashSet<UserAssetEntity>();
        }
        public int Id { get; set; }
        public string Email { get; set; }
        public string Salt { get; set; }
        public string Hash { get; set; }
        public int Role { get; set;  }
        public ICollection<UserAssetEntity> Assets { get; set; }
        public int MeasurementSystem { get; set; }
    }
}