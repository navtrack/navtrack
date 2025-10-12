using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Navtrack.Database.Model.Assets;
using Navtrack.Database.Model.Organizations;
using Navtrack.Database.Postgres;

namespace Navtrack.Database.Model.Devices;

[Table("devices")]
public class DeviceEntity : BaseEntity
{
    public required string SerialNumber { get; set; }
    public required string DeviceTypeId { get; set; }
    public required int ProtocolPort { get; set; }
    public Guid AssetId { get; set; }
    public AssetEntity Asset { get; set; }
    public Guid OrganizationId { get; set; }
    public OrganizationEntity Organization { get; set; }
    public DateTime CreatedDate { get; set; }
    public Guid CreatedBy { get; set; }
    public string? OldId { get; set; }
    public List<DeviceMessageEntity> Messages { get; set; } = [];
}