using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Navtrack.DataAccess.Model.Devices.Messages;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;
#pragma warning disable CS8602 // Dereference of a possibly null reference.

namespace Navtrack.Listener.Protocols.Teltonika;

[Service(typeof(ICustomMessageHandler<TeltonikaProtocol>))]
public class TeltonikaMessageHandler : BaseMessageHandler<TeltonikaProtocol>
{
    public override IEnumerable<DeviceMessageDocument>? ParseRange(MessageInput input)
    {
        if (input.ConnectionContext.Device == null)
        {
            if (input.DataMessage.Bytes.Length > 16)
            {
                string imei = Encoding.ASCII.GetString(input.DataMessage.Bytes[2..17]);

                input.ConnectionContext.SetDevice(imei);

                input.NetworkStream.WriteByte(1);
            }

            return null;
        }

        List<DeviceMessageDocument> positions = GetPositions(input);

        if (positions.Count != 0)
        {
            string reply = positions.Count.ToString("X8");

            input.NetworkStream.Write(HexUtil.ConvertHexStringToByteArray(reply));
        }

        return positions;
    }

    private static List<DeviceMessageDocument> GetPositions(MessageInput input)
    {
        List<DeviceMessageDocument> positions = [];

        TeltonikaCodecConfiguration? codecConfiguration = GetCodec(input);

        if (codecConfiguration == null)
        {
            return [];
        }

        int noOfLocations = input.DataMessage.ByteReader.GetOne();

        for (int i = 0; i < noOfLocations; i++)
        {
            DeviceMessageDocument deviceMessageDocument = GetPosition(input, codecConfiguration);

            positions.Add(deviceMessageDocument);
        }

        return positions;
    }

    private static TeltonikaCodecConfiguration? GetCodec(MessageInput input)
    {
        List<int> codecs = Enum.GetValues(typeof(TeltonikaCodec)).Cast<int>().ToList();

        byte[] preamble = input.DataMessage.ByteReader.Get(4);
        uint length = input.DataMessage.ByteReader.Get(4).ToUInt4();
        byte codecId = input.DataMessage.ByteReader.GetOne();
        
        if (codecs.Contains(codecId))
        {
            TeltonikaCodec? codec = (TeltonikaCodec)codecId;
            
            TeltonikaCodecConfiguration? teltonikaCodecConfiguration = TeltonikaCodecConfiguration.GetAll[codec.Value];

            return teltonikaCodecConfiguration;
        }
        
        return null;
    }

    private static DeviceMessageDocument GetPosition(MessageInput input,
        TeltonikaCodecConfiguration? teltonikaCodecConfiguration)
    {
        DeviceMessageDocument deviceMessageDocument = new()
        {
            Position = new PositionElement(),
            Event = new EventElement()
        };

        deviceMessageDocument.Position.Date = DateTime.UnixEpoch
            .AddMilliseconds(input.DataMessage.ByteReader.Get(8).ToSLong8());

        deviceMessageDocument.Event.Priority = input.DataMessage.ByteReader.GetOne();

        deviceMessageDocument.Position.Longitude = GetCoordinate(input.DataMessage.ByteReader.Get(4));
        deviceMessageDocument.Position.Latitude = GetCoordinate(input.DataMessage.ByteReader.Get(4));
        deviceMessageDocument.Position.Altitude = input.DataMessage.ByteReader.Get(2).ToSShort2();
        deviceMessageDocument.Position.Heading = input.DataMessage.ByteReader.Get(2).ToSShort2();
        deviceMessageDocument.Position.Satellites = input.DataMessage.ByteReader.GetOne();
        deviceMessageDocument.Position.Speed = input.DataMessage.ByteReader.Get(2).ToSShort2();

        ushort eventId = teltonikaCodecConfiguration.MainEventIdLength == 2
            ? input.DataMessage.ByteReader.Get(2).ToUShort2()
            : input.DataMessage.ByteReader.Get(1).ToUByte1();

        deviceMessageDocument.Data["EventId"] = eventId.ToString();

        if (teltonikaCodecConfiguration.HasGenerationType)
        {
            byte generationType = input.DataMessage.ByteReader.GetOne();
        }

        byte[] dataPacketsCount = input.DataMessage.ByteReader.Get(teltonikaCodecConfiguration.DataPacketCountBytes);

        MapDataPackets(deviceMessageDocument, input.DataMessage.ByteReader, teltonikaCodecConfiguration,
            1); // 1 byte events
        MapDataPackets(deviceMessageDocument, input.DataMessage.ByteReader, teltonikaCodecConfiguration,
            2); // 2 bytes events
        MapDataPackets(deviceMessageDocument, input.DataMessage.ByteReader, teltonikaCodecConfiguration,
            4); // 4 bytes events
        MapDataPackets(deviceMessageDocument, input.DataMessage.ByteReader, teltonikaCodecConfiguration,
            8); // 8 bytes events
        MapDataPackets(deviceMessageDocument, input.DataMessage.ByteReader,
            teltonikaCodecConfiguration); // variable bytes events

        // deviceMessageDocument.Teltonika = GetTeltonikaObject(eventId, priority, deviceMessageDocument);

        return deviceMessageDocument;
    }

    private static double GetCoordinate(byte[] coordinate)
    {
        double convertedCoordinate = coordinate.ToSInt4();
        string binary = Convert.ToString(coordinate[0], 2).PadLeft(8, '0');
        bool isNegative = binary[0] == 1;

        if (isNegative)
        {
            convertedCoordinate *= -1;
        }

        return convertedCoordinate / 10000000;
    }

    private static void MapDataPackets(DeviceMessageDocument deviceMessageDocument, ByteReader input,
        TeltonikaCodecConfiguration? teltonikaCodecConfiguration,
        short? dataPacketBytes = null)
    {
        short numberOfDataPackets = GetNumberOfDataPackets(input, teltonikaCodecConfiguration, dataPacketBytes);

        for (int i = 0; i < numberOfDataPackets; i++)
        {
            short id = teltonikaCodecConfiguration.DataPacketIdBytes == 1
                ? input.GetOne()
                : input.Get(2).ToSShort2();
            short length = dataPacketBytes ?? input.Get(2).ToSShort2();
            byte[] value = input.Get(length);


            MapDataPacket(id, value, deviceMessageDocument);
        }
    }

    private static void MapDataPacket(short id, byte[] value, DeviceMessageDocument deviceMessageDocument)
    {
        try
        {
            switch (id)
            {
                // Permanent I/O elements
                case 1:
                    deviceMessageDocument.Data["DigitalInput1"] = $"{value.ToBoolean()}";
                    return;
                case 11:
                    deviceMessageDocument.Data["ICCID1"] = $"{value.ToULong8()}";
                    return;
                case 14:
                    deviceMessageDocument.Data["ICCID2"] = $"{value.ToULong8()}";
                    return;
                case 15:
                    deviceMessageDocument.Data["EcoScore"] =
                        $"{value.ToUShort2() * 0.01}";
                    return;
                case 16:
                    deviceMessageDocument.Data["TotalOdometer"] = $"{value.ToSInt4()}";
                    return;
                case 17:
                    deviceMessageDocument.Data["AxisX"] = $"{value.ToSShort2()}";
                    return;
                case 21:
                    deviceMessageDocument.Data["GSMSignal"] = $"{value.ToUByte1()}";
                    return;
                case 24:
                    deviceMessageDocument.Data["Speed"] = $"{value.ToUShort2()}";
                    return;
                case 66:
                    deviceMessageDocument.Data["ExternalVoltage"] =
                        $"{value.ToSShort2() * 0.001}";
                    return;
                case 67:
                    deviceMessageDocument.Data["BatteryVoltage"] =
                        $"{value.ToUShort2() * 0.001}";
                    return;
                case 68:
                    deviceMessageDocument.Data["BatteryCurrent"] =
                        $"{value.ToUShort2() * 0.001}";
                    return;
                case 69:
                    deviceMessageDocument.Data["GNSSStatus"] = $"{value.ToUByte1()}";
                    return;
                case 113:
                    deviceMessageDocument.Data["BatteryLevel"] = $"{value.ToUByte1()}";
                    return;
                case 181:
                    deviceMessageDocument.Data["PDOP"] = $"{value.ToUShort2() * 0.1}";
                    return;
                case 182:
                    deviceMessageDocument.Data["HDOP"] = $"{value.ToUShort2() * 0.1}";
                    return;
                case 199:
                    deviceMessageDocument.Data["TripOdometer"] = $"{value.ToUInt4()}";
                    return;
                case 200:
                    deviceMessageDocument.Data["SleepMode"] = $"{value.ToUByte1()}";
                    return;
                case 206:
                    deviceMessageDocument.Data["GSMAreaCode"] = $"{value.ToUShort2()}";
                    return;
                case 239:
                    deviceMessageDocument.Data["Ignition"] = $"{value.ToBoolean()}";
                    return;
                case 240:
                    deviceMessageDocument.Data["Movement"] = $"{value.ToBoolean()}";
                    return;
                case 241:
                    deviceMessageDocument.Data["ActiveGSMOperator"] =
                        $"{value.ToUInt4()}";
                    return;
                case 636:
                    deviceMessageDocument.Data["LTECellId"] = $"{value.ToUInt4()}";
                    return;
                
                // Event I/O elements
                case 250:
                    deviceMessageDocument.Data["Trip"] = $"{value.ToUByte1()}";
                    return;

                // OBD elements
                case 30:
                    deviceMessageDocument.Data["DtcCount"] = $"{value.ToUByte1()}";
                    return;
                case 31:
                    deviceMessageDocument.Data["EngineLoad"] = $"{value.ToUByte1()}";
                    return;
                case 32:
                    deviceMessageDocument.Data["CoolantTemperature"] =
                        $"{value.ToSByte1()}";
                    return;
                case 33:
                    deviceMessageDocument.Data["ShortFuelTrim"] = $"{value.ToSByte1()}";
                    return;
                case 34:
                    deviceMessageDocument.Data["FuelPressure"] = $"{value.ToULong8()}";
                    return;
                case 35:
                    deviceMessageDocument.Data["IntakeMAP"] = $"{value.ToUByte1()}";
                    return;
                case 36:
                    deviceMessageDocument.Data["EngineRpm"] = $"{value.ToUShort2()}";
                    return;
                case 37:
                    deviceMessageDocument.Data["VehicleSpeed"] = $"{value.ToUByte1()}";
                    return;
                case 38:
                    deviceMessageDocument.Data["TimingAdvance"] = $"{value.ToSByte1()}";
                    return;
                case 39:
                    deviceMessageDocument.Data["IntakeAirTemperature"] =
                        $"{value.ToSByte1()}";
                    return;
                case 40:
                    deviceMessageDocument.Data["MAF"] = $"{value.ToUShort2()}";
                    return;
                case 41:
                    deviceMessageDocument.Data["ThrottlePosition"] =
                        $"{value.ToUByte1()}";
                    return;
                case 42:
                    deviceMessageDocument.Data["RuntimeSinceEngineStart"] =
                        $"{value.ToUShort2()}";
                    return;
                case 43:
                    deviceMessageDocument.Data["DistanceTraveledMILOn"] =
                        $"{value.ToUShort2()}";
                    return;
                case 44:
                    deviceMessageDocument.Data["RelativeFuelRailPressure"] =
                        $"{value.ToUShort2()}";
                    return;
                case 45:
                    deviceMessageDocument.Data["DirectFuelRailPressure"] =
                        $"{value.ToUShort2()}";
                    return;
                case 46:
                    deviceMessageDocument.Data["CommandedEGR"] = $"{value.ToUByte1()}";
                    return;
                case 47:
                    deviceMessageDocument.Data["EGRError"] = $"{value.ToSByte1()}";
                    return;
                case 48:
                    deviceMessageDocument.Data["FuelLevel"] = $"{value.ToUByte1()}";
                    return;
                case 49:
                    deviceMessageDocument.Data["DistanceSinceCodesClear"] =
                        $"{value.ToUShort2()}";
                    return;
                case 50:
                    deviceMessageDocument.Data["BarometicPressure"] =
                        $"{value.ToUByte1()}";
                    return;
                case 51:
                    deviceMessageDocument.Data["ControlModuleVoltage"] =
                        $"{value.ToUShort2()}";
                    return;
                case 52:
                    deviceMessageDocument.Data["AbsoluteLoadValue"] =
                        $"{value.ToUShort2()}";
                    return;
                case 53:
                    deviceMessageDocument.Data["AmbientAirTemperature"] =
                        $"{value.ToSByte1()}";
                    return;
                case 54:
                    deviceMessageDocument.Data["TimeRunWithMILOn"] =
                        $"{value.ToUShort2()}";
                    return;
                case 55:
                    deviceMessageDocument.Data["TimeSinceCodesCleared"] =
                        $"{value.ToUShort2()}";
                    return;
                case 56:
                    deviceMessageDocument.Data["AbsoluteFuelRailPressure"] =
                        $"{value.ToUShort2()}";
                    return;
                case 57:
                    deviceMessageDocument.Data["HybridBatteryPackLife"] =
                        $"{value.ToUByte1()}";
                    return;
                case 58:
                    deviceMessageDocument.Data["EngineOilTemperature"] =
                        $"{value.ToUByte1()}";
                    return;
                case 59:
                    deviceMessageDocument.Data["FuelInjectionTiming"] =
                        $"{value.ToSShort2()}";
                    return;
                case 60:
                    deviceMessageDocument.Data["FuelRate"] = $"{value.ToUShort2()}";
                    return;
                case 256:
                    deviceMessageDocument.Data["VIN"] =
                        StringUtil.ConvertByteArrayToString(value);
                    return;
                case 281:
                    deviceMessageDocument.Data["FaultCodes"] =
                        $"{StringUtil.ConvertByteArrayToString(value)}";
                    return;
                case 540:
                    deviceMessageDocument.Data["ThrottlePositionGroup"] =
                        $"{value.ToUByte1()}";
                    return;
                case 541:
                    deviceMessageDocument.Data["CommandedEquivalenceRatio"] =
                        $"{value.ToUByte1()}";
                    return;
                case 542:
                    deviceMessageDocument.Data["IntakeMAP2Bytes"] =
                        $"{value.ToUShort2()}";
                    return;
                case 543:
                    deviceMessageDocument.Data["HybridSystemVoltage"] =
                        $"{value.ToUShort2()}";
                    return;
                case 544:
                    deviceMessageDocument.Data["HybridSystemCurrent"] =
                        $"{value.ToSShort2()}";
                    return;
                case 759:
                    deviceMessageDocument.Data["FuelType"] = $"{value.ToUByte1()}";
                    return;

                // OBD OEM elements
                case 389:
                    deviceMessageDocument.Data["TotalMileage"] = $"{value.ToUInt4()}";
                    return;
                case 390:
                    deviceMessageDocument.Data["FuelLevel"] = $"{value.ToUInt4() * 0.1}";
                    return;
                case 402:
                    deviceMessageDocument.Data["DistanceUntilService"] =
                        $"{value.ToUInt4()}";
                    return;
                case 410:
                    deviceMessageDocument.Data["BatteryChargeState"] =
                        $"{value.ToUByte1()}";
                    return;
                case 411:
                    deviceMessageDocument.Data["BatteryChargeLevel"] =
                        $"{value.ToUByte1()}";
                    return;
                case 412:
                    deviceMessageDocument.Data["BatteryPowerConsumption"] =
                        $"{value.ToUShort2()}";
                    return;
                case 755:
                    deviceMessageDocument.Data["RemainingDistance"] =
                        $"{value.ToUShort2()}";
                    return;
                case 1151:
                    deviceMessageDocument.Data["BatteryStateOfHealth"] =
                        $"{value.ToUShort2()}";
                    return;
                case 1152:
                    deviceMessageDocument.Data["BatteryTemperature"] =
                        $"{value.ToSShort2()}";
                    return;
                
                // CAN adapters
                case 90:
                    deviceMessageDocument.Data["DoorStatus"] = $"{value.ToUShort2()}";
                    return;
                case 100:
                    deviceMessageDocument.Data["ProgramNumber"] = $"{value.ToUInt4()}";
                    return;
                case 123:
                    deviceMessageDocument.Data["ControlStateFlags"] = $"{value.ToUInt4()}";
                    return;
                case 124:
                    deviceMessageDocument.Data["AgriculturalMachineryFlags"] = $"{value.ToULong8()}";
                    return;
                case 132:
                    deviceMessageDocument.Data["SecurityStateFlags"] = $"{value.ToULong8()}";
                    return;
                case 232:
                    deviceMessageDocument.Data["CNGStatus"] = $"{value.ToUByte1()}";
                    return;
                case 517:
                    deviceMessageDocument.Data["SecurityStateFlagsP4"] = $"{value.ToULong8()}";
                    return;
                    

                default:
                    string[] array = HexUtil.ConvertByteArrayToHexStringArray(value);
                    Array.Reverse(array);

                    deviceMessageDocument.Extra[$"{id}"] = HexUtil.ConvertHexStringArrayToHexString(array);

                    return;
            }
        }
        catch (Exception)
        {
            string[] array = HexUtil.ConvertByteArrayToHexStringArray(value);
            Array.Reverse(array);

            deviceMessageDocument.Extra[$"{id}"] =
                HexUtil.ConvertHexStringArrayToHexString(array);
        }
    }

    private static short GetNumberOfDataPackets(ByteReader input,
        TeltonikaCodecConfiguration? teltonikaCodecConfiguration,
        int? dataPacketBytes)
    {
        if (dataPacketBytes != null)
        {
            return teltonikaCodecConfiguration.DataPacketBytes == 1
                ? input.GetOne()
                : input.Get(2).ToSShort2();
        }

        if (teltonikaCodecConfiguration.HasVariableDataPackets)
        {
            return input.Get(2).ToSShort2();
        }

        return 0;
    }
}