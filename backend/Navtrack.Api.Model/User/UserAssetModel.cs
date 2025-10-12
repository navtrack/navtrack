using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Navtrack.Database.Model.Assets;

namespace Navtrack.Api.Model.User;

public class UserAssetModel
{
    [Required]
    public string AssetId { get; set; }
    
    [Required]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public AssetUserRole UserRole { get; set; }
}