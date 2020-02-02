namespace Navtrack.DataAccess.Model
{
    public class UserDevice : IUserRelation
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public int DeviceId { get; set; }
        public Device Device { get; set; }
        public int RoleId { get; set; }
    }
}