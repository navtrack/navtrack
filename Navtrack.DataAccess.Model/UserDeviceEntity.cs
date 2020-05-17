using Navtrack.DataAccess.Model.Common;

namespace Navtrack.DataAccess.Model
{
    public class UserDeviceEntity : EntityAudit, IUserRelation
    {
        public int UserId { get; set; }
        public UserEntity User { get; set; }
        public int DeviceId { get; set; }
        public DeviceEntity Device { get; set; }
        public int RoleId { get; set; }
    }
}