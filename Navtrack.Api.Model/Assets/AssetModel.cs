using System;
using System.ComponentModel.DataAnnotations;
using Navtrack.Api.Model.Locations;

namespace Navtrack.Api.Model.Assets;

public class AssetModel
{
    [Required]
    public string Id { get; set; }
        
    [Required]
    public string ShortId => Id.Substring(Math.Max(0, Id.Length - 6));
        
    [Required]
    public string Name { get; set; }
         
    [Required]
    public bool Online => Location != null && Location.DateTime > DateTime.Now.AddMinutes(-2);
        
    [Required]
    public int MaxSpeed => 400; // TODO update this property

    public LocationModel Location { get; set; }
}