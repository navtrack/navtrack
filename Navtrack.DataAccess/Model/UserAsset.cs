namespace Navtrack.DataAccess.Model
{
    public class UserAsset : IUserRelation
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public int AssetId { get; set; }
        public Asset Asset { get; set; }
        public int RoleId { get; set; }
    }
}