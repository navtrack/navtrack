using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Navtrack.Api.Model.Devices;
using Navtrack.Api.Model.Positions;

namespace Navtrack.Api.Model.Assets;

public class AssetModel
{
    [Required]
    public string Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public bool Online { get; set; }

    [Required]
    public int MaxSpeed => 400; // TODO update this property

    public PositionModel? Position { get; set; }

    [Required]
    public DeviceModel Device { get; set; }

    public IEnumerable<AssetUserModel>? Users { get; set; }
}