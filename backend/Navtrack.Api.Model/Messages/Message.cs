using System;
using System.ComponentModel.DataAnnotations;
using Navtrack.DataAccess.Model.Devices.Messages;

namespace Navtrack.Api.Model.Messages;

public class Message
{
    public string Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public MessagePriority? Priority { get; set; }

    [Required]
    public MessagePosition Position { get; set; }

    public MessageDevice? Device { get; set; }
    public MessageVehicle? Vehicle { get; set; }
    public MessageGsm? Gsm { get; set; }
}