using Navtrack.DataAccess.Model.Common;

namespace Navtrack.DataAccess.Model
{
    public class Configuration : EntityAudit, IEntity
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}