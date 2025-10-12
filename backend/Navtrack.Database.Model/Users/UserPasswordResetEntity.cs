using System;
using System.ComponentModel.DataAnnotations.Schema;
using Navtrack.Database.Postgres;

namespace Navtrack.Database.Model.Users;

[Table("users_password_resets")]
public class UserPasswordResetEntity : BaseEntity
{
    public string Email { get; set; }
    public string IpAddress { get; set; }
    public bool Invalid { get; set; }
    public DateTime CreatedDate { get; set; }
    public Guid CreatedBy { get; set; }
}