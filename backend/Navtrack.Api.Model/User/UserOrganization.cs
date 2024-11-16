using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Navtrack.DataAccess.Model.Organizations;

namespace Navtrack.Api.Model.User;

public class UserOrganization
{
    [Required]
    public string OrganizationId { get; set; }
    
    [Required]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public OrganizationUserRole UserRole { get; set; }
}