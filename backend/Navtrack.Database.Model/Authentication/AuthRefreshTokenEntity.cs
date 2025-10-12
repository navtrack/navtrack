using System;
using System.ComponentModel.DataAnnotations.Schema;
using Navtrack.Database.Postgres;

namespace Navtrack.Database.Model.Authentication;

[Table("auth_refresh_tokens")]
public class AuthRefreshTokenEntity : BaseEntity
{
    public DateTime CreationTime { get; set; }
    public int Lifetime { get; set; }
    public DateTime? ConsumedTime { get; set; }
    [Column(TypeName = "jsonb")]
    public string AccessToken { get; set; }
    public int Version { get; set; }
    public string Hash { get; set; }
    public string JwtId { get; set; }
    public DateTime ExpiryTime { get; set; }
    public string SubjectId { get; set; }
    public string ClientId { get; set; }
}