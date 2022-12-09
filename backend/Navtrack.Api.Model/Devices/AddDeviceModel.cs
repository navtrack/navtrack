using System.ComponentModel.DataAnnotations;

namespace Navtrack.Api.Model.Devices;

public class AddDeviceModel
{
    [Required]
    public string SerialNumber { get; set; }

    [Required]
    public string DeviceTypeId { get; set; }
}