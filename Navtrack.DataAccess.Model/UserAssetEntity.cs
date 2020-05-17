using Navtrack.DataAccess.Model.Common;

namespace Navtrack.DataAccess.Model
{
    public class UserAssetEntity : EntityAudit, IUserRelation
    {
        public int UserId { get; set; }
        public UserEntity User { get; set; }
        public int AssetId { get; set; }
        public AssetEntity Asset { get; set; }
        public int RoleId { get; set; }
    }
}