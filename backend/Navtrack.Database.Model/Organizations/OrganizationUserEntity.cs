using System;
using System.ComponentModel.DataAnnotations.Schema;
using Navtrack.Database.Model.Users;
using Navtrack.Database.Postgres;

namespace Navtrack.Database.Model.Organizations;

[Table("organizations_users")]
public class OrganizationUserEntity : BaseEntity
{
    public Guid UserId { get; set; }
    public UserEntity User { get; set; }
    public Guid OrganizationId { get; set; }
    public OrganizationEntity Organization { get; set; }
    
    public OrganizationUserRole UserRole { get; set; }
    public DateTime CreatedDate { get; set; }
    public Guid? CreatedBy { get; set; }
}