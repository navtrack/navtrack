using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Navtrack.DataAccess.Model.Assets;

namespace Navtrack.Api.Model.Assets;

public class AssetUser
{  
    [Required]
    public string Email { get; set; }
    
    [Required]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public AssetUserRole UserRole { get; set; }

    [Required]
    public string UserId { get; set; }

    [Required]
    public DateTime CreatedDate { get; set; }
}