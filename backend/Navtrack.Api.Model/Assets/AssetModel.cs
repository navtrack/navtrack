using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Navtrack.Api.Model.Messages;
using DeviceModel = Navtrack.Api.Model.Devices.DeviceModel;

namespace Navtrack.Api.Model.Assets;

public class AssetModel
{
    [Required]
    public string Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public string OwnerId { get; set; }

    [Required]
    public bool Online { get; set; }

    [Required]
    public int MaxSpeed => 400; // TODO update this property

    public MessageModel? LastMessage { get; set; }
    public MessageModel? LastPositionMessage { get; set; }

    public DeviceModel? Device { get; set; }

    public IEnumerable<AssetUserRoleModel>? UserRoles { get; set; }
}