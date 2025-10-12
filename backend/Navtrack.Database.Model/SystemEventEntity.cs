using System;
using System.ComponentModel.DataAnnotations.Schema;
using Navtrack.Database.Postgres;

namespace Navtrack.Database.Model;

[Table("system_events")]
public class SystemEventEntity : BaseEntity
{
    public DateTime CreatedDate { get; set; }
    public string Type { get; set; }
    public Guid? UserId { get; set; }
    public string Payload { get; set; }
    public string? OldId { get; set; }
}