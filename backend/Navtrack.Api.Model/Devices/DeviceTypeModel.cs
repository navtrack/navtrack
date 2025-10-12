using System.ComponentModel.DataAnnotations;
using Navtrack.Api.Model.Protocols;

namespace Navtrack.Api.Model.Devices;

public class DeviceTypeModel
{
    [Required]
    public string Id { get; set; }
    [Required]
    public string Manufacturer { get; set; }
    [Required]
    public string Model { get; set; }
    [Required]
    public string DisplayName => $"{Manufacturer} {Model}";
    [Required]
    public ProtocolModel Protocol { get; set; }
}