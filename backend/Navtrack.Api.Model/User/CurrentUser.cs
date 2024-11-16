using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Navtrack.DataAccess.Model.Shared;

namespace Navtrack.Api.Model.User;

public class CurrentUser
{
    [Required]
    public string Id { get; set; }

    [Required]
    public string Email { get; set; }

    [Required]
    public UnitsType Units { get; set; }
    
    public IEnumerable<UserAsset> Assets { get; set; }
    
    public IEnumerable<UserOrganization> Organizations { get; set; }
    
    public UserAuthentication Authentication { get; set; }
}