using System.Collections.Generic;
using Navtrack.DataAccess.Model.Common;

namespace Navtrack.DataAccess.Model
{
    public class User : EntityAudit, IEntity
    {
        public User()
        {
            Assets = new HashSet<UserAsset>();
            Devices = new HashSet<UserDevice>();
        }
        public int Id { get; set; }
        public string Email { get; set; }
        public string Salt { get; set; }
        public string Hash { get; set; }
        public int Role { get; set;  }
        public ICollection<UserAsset> Assets { get; set; }
        public ICollection<UserDevice> Devices { get; set; }
    }
}