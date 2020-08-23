using System;
using System.Collections.Generic;
using Navtrack.DataAccess.Model.Common;

namespace Navtrack.DataAccess.Model
{
    public class DeviceConnectionEntity : IEntity
    {
        public DeviceConnectionEntity()
        {
            Messages = new HashSet<DeviceConnectionMessageEntity>();
        }
        public int Id { get; set; }
        public DateTime OpenedAt { get; set; }
        public DateTime? ClosedAt { get; set; }
        public int ProtocolPort { get; set; }
        public string RemoteEndPoint { get; set; }
        public int? DeviceId { get; set; }
        public DeviceEntity Device { get; set; }
        public ICollection<DeviceConnectionMessageEntity> Messages { get; set; }
    }
}