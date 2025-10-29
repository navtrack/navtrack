using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Navtrack.Database.Model.Assets;
using Navtrack.Database.Postgres;
using NetTopologySuite.Geometries;
using NpgsqlTypes;

namespace Navtrack.Database.Model.Devices;

[Table("devices_messages")]
public class DeviceMessageEntity : BaseEntity
{
    public Guid AssetId { get; set; }
    public AssetEntity Asset { get; set; }
    public Guid DeviceId { get; set; }
    public DeviceEntity Device { get; set; }
    public Guid? ConnectionId { get; set; }
    public DeviceConnectionEntity Connection { get; set; }
    public DateTime CreatedDate { get; set; }
    public MessagePriority? MessagePriority { get; set; }

    public DateTime Date { get; set; }

    public NpgsqlPoint Coordinates { get; set; }
    public Point? NewCoordinates { get; set; }
    public string OldMessage { get; set; }

    [NotMapped]
    public double Latitude
    {
        get => NewCoordinates?.Y ?? 0;
        set
        {
            if (NewCoordinates == null)
            {
                NewCoordinates = new Point(NewCoordinates?.X ?? 0, value);
            }
            else
            {
                NewCoordinates.Y = value;
            }
        }
    }

    [NotMapped]
    public double Longitude
    {
        get => NewCoordinates?.X ?? 0;
        set
        {
            if (NewCoordinates == null)
            {
                NewCoordinates = new Point(value, NewCoordinates?.Y ?? 0);
            }
            else
            {
                NewCoordinates.X = value;
            }
        }
    }

    public short? Speed { get; set; }
    public short? Heading { get; set; }
    public short? Altitude { get; set; }
    public short? Satellites { get; set; }
    public float? PDOP { get; set; }
    public float? HDOP { get; set; }
    public bool? Valid { get; set; }

    /// <summary>
    /// Odometer in meters.
    /// </summary>
    public int? DeviceOdometer { get; set; }

    /// <summary>
    /// Battery level (0%-100%)
    /// </summary>
    public byte? DeviceBatteryLevel { get; set; }

    /// <summary>
    /// Battery voltage (V)
    /// </summary>
    public float? DeviceBatteryVoltage { get; set; }

    /// <summary>
    /// Battery current (A)
    /// </summary>
    public float? DeviceBatteryCurrent { get; set; }

    /// <summary>
    /// In km.
    /// </summary>
    public int? VehicleOdometer { get; set; }

    public bool? VehicleIgnition { get; set; }

    /// <summary>
    /// Duration in seconds for the ignition ON.
    /// </summary>
    public int? VehicleIgnitionDuration { get; set; }

    public float? VehicleFuelConsumption { get; set; }
    public float? VehicleVoltage { get; set; }

    /// GSM standard TS 27.007, section 8.5 values
    /// 0        -113 dBm or less - low signal
    /// 1        -111 dBm  
    /// 2...30   -109 ... -53 dBm  
    /// 31       -51 dBm or greater - high signal
    /// 99       not known or not detectable
    public short? GSMSignalStrength { get; set; }

    /// <summary>
    /// Values from 1 to 5
    /// </summary>
    public byte? GSMSignalLevel { get; set; }

    public string? GSMMobileCountryCode { get; set; }
    public string? GSMMobileNetworkCode { get; set; }
    public string? GSMLocationAreaCode { get; set; }
    public int? GSMCellId { get; set; }
    public int? GSMLteCellId { get; set; }

    public string? OldId { get; set; }

    [NotMapped]
    public Dictionary<string, string>? AdditionalDataDic { get; set; }

    public string? AdditionalData { get; set; }
}