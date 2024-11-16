using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Navtrack.DataAccess.Model.Assets;

namespace Navtrack.Api.Model.User;

public class UserAsset
{
    [Required]
    public string AssetId { get; set; }
    
    [Required]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public AssetUserRole UserRole { get; set; }
}