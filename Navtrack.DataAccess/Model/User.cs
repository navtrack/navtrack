namespace Navtrack.DataAccess.Model
{
    public class User : IEntity
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Salt { get; set; }
        public string Hash { get; set; }
    }
}