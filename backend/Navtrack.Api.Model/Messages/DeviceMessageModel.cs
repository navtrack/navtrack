using System;
using System.ComponentModel.DataAnnotations;
using Navtrack.Database.Model.Devices;

namespace Navtrack.Api.Model.Messages;

public class DeviceMessageModel
{
    public string Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public MessagePriority? Priority { get; set; }

    [Required]
    public PositionDataModel Position { get; set; }

    public DeviceDataModel? Device { get; set; }
    public VehicleDataModel? Vehicle { get; set; }
    public GsmDataModel? Gsm { get; set; }
}