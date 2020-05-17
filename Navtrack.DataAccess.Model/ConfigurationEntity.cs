using Navtrack.DataAccess.Model.Common;

namespace Navtrack.DataAccess.Model
{
    public class ConfigurationEntity : EntityAudit, IEntity
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}