using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Navtrack.Database.Model.Teams;

namespace Navtrack.Api.Model.User;

public class UserTeamModel
{
    [Required]
    public string TeamId { get; set; }
    
    [Required]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public TeamUserRole UserRole { get; set; }
}