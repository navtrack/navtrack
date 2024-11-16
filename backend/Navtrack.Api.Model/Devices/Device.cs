using System.ComponentModel.DataAnnotations;

namespace Navtrack.Api.Model.Devices;

public class Device
{
    [Required]
    public string Id { get; set; }

    [Required]
    public string SerialNumber { get; set; }

    [Required]
    public DeviceType DeviceType { get; set; }
    
    [Required]
    public bool Active { get; set; }

    public int? Positions { get; set; }
}