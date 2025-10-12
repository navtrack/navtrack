using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Navtrack.Database.Model.Organizations;

namespace Navtrack.Api.Model.Organizations;

public class OrganizationUserModel
{  
    [Required]
    public string Email { get; set; }
    
    [Required]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public OrganizationUserRole UserRole { get; set; }

    [Required]
    public string UserId { get; set; }

    [Required]
    public DateTime CreatedDate { get; set; }
}