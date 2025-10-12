using System;

namespace Navtrack.Listener.Models;

public class Device
{
    public Device(string serialNumber)
    {
        SerialNumber = serialNumber;
    }

    public string SerialNumber { get; init; }
    public Guid? AssetId { get; set; }
    public Guid? DeviceId { get; set; }
    public DateTime? MaxDate { get; set; }
}