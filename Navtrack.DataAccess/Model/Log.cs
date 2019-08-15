namespace Navtrack.DataAccess.Model
{
    public class Log : EntityAudit
    {
        public int Id { get; set; }
        public string Data { get; set; }
    }
}