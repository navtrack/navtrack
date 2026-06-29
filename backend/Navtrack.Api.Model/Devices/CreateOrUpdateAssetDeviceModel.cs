using System.ComponentModel.DataAnnotations;

namespace Navtrack.Api.Model.Devices;

public class CreateOrUpdateAssetDeviceModel
{
    [Required(ErrorMessage = "serialNumber.required")]
    public string SerialNumber { get; set; }

    [Required(ErrorMessage = "deviceTypeId.required")]
    public string DeviceTypeId { get; set; }
}