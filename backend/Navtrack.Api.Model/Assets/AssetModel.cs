using System;
using System.ComponentModel.DataAnnotations;
using Navtrack.Api.Model.Devices;
using Navtrack.Api.Model.Locations;

namespace Navtrack.Api.Model.Assets;

public class AssetModel
{
    [Required]
    public string Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public bool Online => Location != null && Location.DateTime > DateTime.UtcNow.AddMinutes(-1);

    [Required]
    public int MaxSpeed => 400; // TODO update this property

    public LocationModel Location { get; set; }
    
    [Required]
    public DeviceModel Device { get; set; }
}