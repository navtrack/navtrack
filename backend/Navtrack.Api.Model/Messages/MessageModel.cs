using System;
using System.ComponentModel.DataAnnotations;
using Navtrack.DataAccess.Model.Devices.Messages;

namespace Navtrack.Api.Model.Messages;

public class MessageModel
{
    public string Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public MessagePriority? Priority { get; set; }

    [Required]
    public MessagePositionModel Position { get; set; }

    public MessageDeviceModel? Device { get; set; }
    public MessageVehicleModel? Vehicle { get; set; }
    public MessageGsmModel? Gsm { get; set; }
}