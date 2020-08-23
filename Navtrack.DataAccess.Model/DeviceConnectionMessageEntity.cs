using Navtrack.DataAccess.Model.Common;

namespace Navtrack.DataAccess.Model
{
    public class DeviceConnectionMessageEntity : CreatedEntityAudit, IEntity
    {
        public int Id { get; set; }
        public string Hex { get; set; }
        public int DeviceConnectionId { get; set; }
        
        public DeviceConnectionEntity Connection { get; set; }
    }
}