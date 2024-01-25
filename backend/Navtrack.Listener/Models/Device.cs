using System;
using MongoDB.Bson;

namespace Navtrack.Listener.Models;

public class Device
{
    public Device(string serialNumber)
    {
        SerialNumber = serialNumber;
    }

    public string SerialNumber { get; init; }
    public ObjectId? AssetId { get; set; }
    public ObjectId? DeviceId { get; set; }
    public ObjectId? PositionGroupId { get; set; }
    public DateTime? MaxDate { get; set; }
}