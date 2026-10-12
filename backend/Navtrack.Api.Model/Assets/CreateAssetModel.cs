using System.ComponentModel.DataAnnotations;

namespace Navtrack.Api.Model.Assets;

public class CreateAssetModel
{
    [Required(ErrorMessage = "name.required")]
    public string Name { get; set; }

    [Required(ErrorMessage = "deviceTypeId.required")]
    public string DeviceTypeId { get; set; }

    [Required(ErrorMessage = "serialNumber.required")]
    public string SerialNumber { get; set; }
}