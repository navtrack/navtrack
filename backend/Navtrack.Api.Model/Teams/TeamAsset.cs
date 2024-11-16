using System;
using System.ComponentModel.DataAnnotations;

namespace Navtrack.Api.Model.Teams;

public class TeamAsset
{
    [Required]
    public string AssetId { get; set; }
    
    [Required]
    public string Name { get; set; }
    
    [Required]
    public DateTime CreatedDate { get; set; }
}