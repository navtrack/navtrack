using System;
using System.ComponentModel.DataAnnotations.Schema;
using Navtrack.Database.Model.Users;
using Navtrack.Database.Postgres;

namespace Navtrack.Database.Model.Teams;

[Table("teams_users")]
public class TeamUserEntity : BaseEntity
{
    public Guid UserId { get; set; }
    public UserEntity User { get; set; }
    public Guid TeamId { get; set; }
    public TeamEntity Team { get; set; }
    public TeamUserRole UserRole { get; set; }
    public DateTime CreatedDate { get; set; }
    public Guid CreatedBy { get; set; }
}