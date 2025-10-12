using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Navtrack.Database.Model.Shared;

namespace Navtrack.Api.Model.User;

public class CurrentUserModel
{
    [Required]
    public string Id { get; set; }

    [Required]
    public string Email { get; set; }

    [Required]
    public UnitsType Units { get; set; }
    
    public IEnumerable<UserAssetModel> Assets { get; set; }
    
    public IEnumerable<UserOrganizationModel> Organizations { get; set; }
    
    public UserAuthenticationModel Authentication { get; set; }
}