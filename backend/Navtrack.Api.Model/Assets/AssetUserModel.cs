using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Navtrack.DataAccess.Model.Assets;

namespace Navtrack.Api.Model.Assets;

public class AssetUserModel
{  
    [Required]
    public string Email { get; set; }
    
    [Required]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public AssetRoleType Role { get; set; }

    [Required]
    public string UserId { get; set; }

    public DateTime? CreatedDate { get; set; }
}