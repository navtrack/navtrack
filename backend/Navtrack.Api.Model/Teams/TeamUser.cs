using System;
using System.ComponentModel.DataAnnotations;
using Navtrack.DataAccess.Model.Teams;

namespace Navtrack.Api.Model.Teams;

public class TeamUser
{
    [Required]
    public string UserId { get; set; }
    
    [Required]
    public string Email { get; set; }
    
    [Required]
    public TeamUserRole UserRole { get; set; }
    
    [Required]
    public DateTime CreatedDate { get; set; }
}