namespace Navtrack.DataAccess.Model.Common
{
    public interface IUserRelation
    {
        int UserId { get; set; }
        int RoleId { get; set; }
    }
}