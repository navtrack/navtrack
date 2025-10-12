using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Navtrack.Database.Postgres;

namespace Navtrack.Database.Model.Devices;

[Table("devices_connections")]
public class DeviceConnectionEntity : BaseEntity
{
    public DateTime CreatedDate { get; set; }
    public short Port { get; set; }
    public string? IPAddress { get; set; }
    public List<DeviceConnectionDataEntity>? Data { get; set; } = [];
    public string? OldId { get; set; }
    public List<DeviceMessageEntity>? Messages { get; set; } = [];
}