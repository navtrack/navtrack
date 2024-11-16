using System.ComponentModel.DataAnnotations;

namespace Navtrack.Api.Model.Assets;

public class CreateAsset
{
    [Required]
    public string Name { get; set; }

    [Required]
    public string DeviceTypeId { get; set; }

    [Required]
    public string SerialNumber { get; set; }
}