using System.ComponentModel.DataAnnotations;

namespace Navtrack.Api.Model.Assets;

public class AddAssetModel
{
    [Required]
    public string Name { get; set; }

    [Required]
    public string DeviceTypeId { get; set; }

    [Required]
    public string SerialNumber { get; set; }
}