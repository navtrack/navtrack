using System.ComponentModel.DataAnnotations.Schema;
using Navtrack.Database.Postgres;

namespace Navtrack.Database.Model;

[Table("system_settings")]
public class SystemSettingEntity : BaseEntity
{
    public string Key { get; set; }
    public string Value { get; set; }
    public string? OldId { get; set; }
}