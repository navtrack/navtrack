namespace Navtrack.DataAccess.Model
{
    public interface IUserRelation
    {
        int UserId { get; set; }
        int RoleId { get; set; }
    }
}