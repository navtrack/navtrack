using System;
using System.ComponentModel.DataAnnotations.Schema;
using Navtrack.Database.Postgres;

namespace Navtrack.Database.Model.Devices;

[Table("devices_connections_data")]
public class DeviceConnectionDataEntity : BaseEntity
{
    public Guid ConnectionId { get; set; }
    public DeviceConnectionEntity Connection { get; set; }
    public DateTime CreatedDate { get; set; }
    public byte[] Data { get; set; }
}