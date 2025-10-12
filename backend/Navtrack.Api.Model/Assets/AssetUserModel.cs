using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Navtrack.Database.Model.Assets;

namespace Navtrack.Api.Model.Assets;

public class AssetUserModel
{  
    public string Id { get; set; }
    
    [Required]
    public string Email { get; set; }
    
    [Required]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public AssetUserRole UserRole { get; set; }

    [Required]
    public Guid UserId { get; set; }

    [Required]
    public DateTime CreatedDate { get; set; }

}