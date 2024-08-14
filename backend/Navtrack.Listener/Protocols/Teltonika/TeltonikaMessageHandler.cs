using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Models;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.Teltonika;

[Service(typeof(ICustomMessageHandler<TeltonikaProtocol>))]
public class TeltonikaMessageHandler : BaseMessageHandler<TeltonikaProtocol>
{
    public override IEnumerable<Position>? ParseRange(MessageInput input)
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

        List<TeltonikaPosition> positions = GetPositions(input);

        if (positions.Count != 0)
        {
            string reply = positions.Count.ToString("X8");

            input.NetworkStream.Write(HexUtil.ConvertHexStringToByteArray(reply));
        }

        return positions;
    }

    private static List<TeltonikaPosition> GetPositions(MessageInput input)
    {
        List<TeltonikaPosition> positions = [];

        TeltonikaCodec? codec = GetCodec(input);

        if (codec == null)
        {
            return [];
        }

        TeltonikaCodecConfiguration teltonikaCodecConfiguration = TeltonikaCodecConfiguration.GetAll[codec.Value];

        int noOfLocations = input.DataMessage.ByteReader.GetOne();

        for (int i = 0; i < noOfLocations; i++)
        {
            TeltonikaPosition position = GetPosition(input, codec.Value, teltonikaCodecConfiguration);

            positions.Add(position);
        }

        return positions;
    }

    private static TeltonikaCodec? GetCodec(MessageInput input)
    {
        List<int> codecs = Enum.GetValues(typeof(TeltonikaCodec)).Cast<int>().ToList();

        byte b;

        do
        {
            b = input.DataMessage.ByteReader.GetOne();
        } while (b == 0);

        TeltonikaCodec? codec = null;

        do
        {
            b = input.DataMessage.ByteReader.GetOne();

            if (codecs.Contains(b))
            {
                codec = (TeltonikaCodec)b;
            }
        } while (codec == null);

        return codec;
    }

    private static TeltonikaPosition GetPosition(MessageInput input, TeltonikaCodec teltonikaCodec,
        TeltonikaCodecConfiguration teltonikaCodecConfiguration)
    {
        TeltonikaPosition position = new()
        {
            Device = input.ConnectionContext.Device
        };

        position.Date = DateTime.UnixEpoch
            .AddMilliseconds(input.DataMessage.ByteReader.Get(8).ToSLong8());

        byte priority = input.DataMessage.ByteReader.GetOne();

        position.Longitude = GetCoordinate(input.DataMessage.ByteReader.Get(4));
        position.Latitude = GetCoordinate(input.DataMessage.ByteReader.Get(4));
        position.Altitude = input.DataMessage.ByteReader.Get(2).ToSShort2();
        position.Heading = input.DataMessage.ByteReader.Get(2).ToSShort2();
        position.Satellites = input.DataMessage.ByteReader.GetOne();
        position.Speed = input.DataMessage.ByteReader.Get(2).ToSShort2();

        ushort eventId = teltonikaCodecConfiguration.MainEventIdLength == 2
            ? input.DataMessage.ByteReader.Get(2).ToUShort2()
            : input.DataMessage.ByteReader.Get(1).ToUByte1();

        if (teltonikaCodecConfiguration.HasGenerationType)
        {
            byte generationType = input.DataMessage.ByteReader.GetOne();
        }

        byte[] dataPacketsCount = input.DataMessage.ByteReader.Get(teltonikaCodecConfiguration.DataPacketCountBytes);

        MapDataPackets(position, input.DataMessage.ByteReader, teltonikaCodecConfiguration, 1); // 1 byte events
        MapDataPackets(position, input.DataMessage.ByteReader, teltonikaCodecConfiguration, 2); // 2 bytes events
        MapDataPackets(position, input.DataMessage.ByteReader, teltonikaCodecConfiguration, 4); // 4 bytes events
        MapDataPackets(position, input.DataMessage.ByteReader, teltonikaCodecConfiguration, 8); // 8 bytes events
        MapDataPackets(position, input.DataMessage.ByteReader, teltonikaCodecConfiguration); // variable bytes events

        position.Teltonika = GetTeltonikaObject(eventId, priority, position);

        return position;
    }

    private static object GetTeltonikaObject(ushort eventId, byte priority, TeltonikaPosition position)
    {
        List<TeltonikaDataPacket> dataPackets = position.DataPackets.Where(x => x.Error == null && x.Parsed).ToList();

        List<TeltonikaDataPacket> dataPacketsErrors = position.DataPackets.Where(x => x.Error.GetValueOrDefault()).ToList();

        List<TeltonikaDataPacket> dataPacketsNotParsed = position.DataPackets.Where(x => !x.Parsed).ToList();
        
        return new
        {
            EventId = eventId,
            Priority = priority,
            Permanent = position.PermanentElements,
            OBD = position.OBDElements,
            OEMOBD = position.OEMOBDElements,
            DataPackets = dataPackets.Count == 0 ? null : dataPackets,
            DataPacketsErrors = dataPacketsErrors.Count == 0 ? null : dataPacketsErrors,
            DataPacketsNotParsed = dataPacketsNotParsed.Count == 0 ? null : dataPacketsNotParsed 
        };
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

    private static void MapDataPackets(TeltonikaPosition position, ByteReader input,
        TeltonikaCodecConfiguration teltonikaCodecConfiguration,
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

            TeltonikaDataPacket teltonikaDataPacket = new()
            {
                Id = id,
                Value = value
            };
            position.DataPackets.Add(teltonikaDataPacket);

            MapDataPacket(teltonikaDataPacket, position);
        }
    }

    private static void MapDataPacket(TeltonikaDataPacket teltonikaDataPacket, TeltonikaPosition position)
    {
        try
        {
            teltonikaDataPacket.Parsed = true;
            
            switch (teltonikaDataPacket.Id)
            {
                // Permanent I/O elements
                case 1:
                    position.PermanentElements.DigitalInput1 = teltonikaDataPacket.Value.ToBoolean();
                    return;
                case 11:
                    position.PermanentElements.ICCID1 = teltonikaDataPacket.Value.ToULong8();
                    return;
                case 14:
                    position.PermanentElements.ICCID2 = teltonikaDataPacket.Value.ToULong8();
                    return;
                case 15:
                    position.PermanentElements.EcoScore = teltonikaDataPacket.Value.ToUShort2() * 0.01;
                    return;
                case 16:
                    position.PermanentElements.TotalOdometer = teltonikaDataPacket.Value.ToSInt4();
                    return;
                case 17:
                    position.PermanentElements.AxisX = teltonikaDataPacket.Value.ToSShort2();
                    return;
                case 21:
                    position.PermanentElements.GSMSignal = teltonikaDataPacket.Value.ToUByte1();
                    return;
                case 24:
                    position.PermanentElements.Speed = teltonikaDataPacket.Value.ToUShort2();
                    return;
                case 66:
                    position.PermanentElements.ExternalVoltage = teltonikaDataPacket.Value.ToSShort2() * 0.001;
                    return;
                case 69:
                    position.PermanentElements.GNSSStatus = teltonikaDataPacket.Value.ToUByte1();
                    return;
                case 181:
                    position.PermanentElements.GNSSPDOP = teltonikaDataPacket.Value.ToUShort2() * 0.1;
                    return;
                case 182:
                    position.PermanentElements.GNSSHDOP = teltonikaDataPacket.Value.ToUShort2() * 0.1;
                    return;
                case 199:
                    position.PermanentElements.TripOdometer = teltonikaDataPacket.Value.ToUInt4();
                    return;
                case 200:
                    position.PermanentElements.SleepMode = teltonikaDataPacket.Value.ToUByte1();
                    return;
                case 239:
                    position.PermanentElements.Ignition = teltonikaDataPacket.Value.ToBoolean();
                    return;
                case 240:
                    position.PermanentElements.Movement = teltonikaDataPacket.Value.ToBoolean();
                    return;
                case 241:
                    position.PermanentElements.ActiveGSMOperator = teltonikaDataPacket.Value.ToUInt4();
                    return;

                // OBD elements
                case 30:
                    position.OBDElements.NumberOfDTC = teltonikaDataPacket.Value.ToUByte1();
                    return;
                case 31:
                    position.OBDElements.EngineLoad = teltonikaDataPacket.Value.ToUByte1();
                    return;
                case 32:
                    position.OBDElements.CoolantTemperature = teltonikaDataPacket.Value.ToSByte1();
                    return;
                case 33:
                    position.OBDElements.ShortFuelTrim = teltonikaDataPacket.Value.ToSByte1();
                    return;
                case 34:
                    position.OBDElements.FuelPressure = teltonikaDataPacket.Value.ToULong8();
                    return;
                case 35:
                    position.OBDElements.IntakeMAP = teltonikaDataPacket.Value.ToUByte1();
                    return;
                case 36:
                    position.OBDElements.EngineRPM = teltonikaDataPacket.Value.ToUShort2();
                    return;
                case 37:
                    position.OBDElements.VehicleSpeed = teltonikaDataPacket.Value.ToUByte1();
                    return;
                case 38:
                    position.OBDElements.TimingAdvance = teltonikaDataPacket.Value.ToSByte1();
                    return;
                case 39:
                    position.OBDElements.IntakeAirTemperature = teltonikaDataPacket.Value.ToSByte1();
                    return;
                case 40:
                    position.OBDElements.MAF = teltonikaDataPacket.Value.ToUShort2();
                    return;
                case 41:
                    position.OBDElements.ThrottlePosition = teltonikaDataPacket.Value.ToUByte1();
                    return;
                case 42:
                    position.OBDElements.RuntimeSinceEngineStart = teltonikaDataPacket.Value.ToUShort2();
                    return;
                case 43:
                    position.OBDElements.DistanceTraveledMILOn = teltonikaDataPacket.Value.ToUShort2();
                    return;
                case 44:
                    position.OBDElements.RelativeFuelRailPressure = teltonikaDataPacket.Value.ToUShort2();
                    return;
                case 45:
                    position.OBDElements.DirectFuelRailPressure = teltonikaDataPacket.Value.ToUShort2();
                    return;
                case 46:
                    position.OBDElements.CommandedEGR = teltonikaDataPacket.Value.ToUByte1();
                    return;
                case 47:
                    position.OBDElements.EGRError = teltonikaDataPacket.Value.ToSByte1();
                    return;
                case 48:
                    position.OBDElements.FuelLevel = teltonikaDataPacket.Value.ToUByte1();
                    return;
                case 49:
                    position.OBDElements.DistanceSinceCodesClear = teltonikaDataPacket.Value.ToUShort2();
                    return;
                case 50:
                    position.OBDElements.BarometicPressure = teltonikaDataPacket.Value.ToUByte1();
                    return;
                case 51:
                    position.OBDElements.ControlModuleVoltage = teltonikaDataPacket.Value.ToUShort2();
                    return;
                case 52:
                    position.OBDElements.AbsoluteLoadValue = teltonikaDataPacket.Value.ToUShort2();
                    return;
                case 53:
                    position.OBDElements.AmbientAirTemperature = teltonikaDataPacket.Value.ToSByte1();
                    return;
                case 54:
                    position.OBDElements.TimeRunWithMILOn = teltonikaDataPacket.Value.ToUShort2();
                    return;
                case 55:
                    position.OBDElements.TimeSinceCodesCleared = teltonikaDataPacket.Value.ToUShort2();
                    return;
                case 56:
                    position.OBDElements.AbsoluteFuelRailPressure = teltonikaDataPacket.Value.ToUShort2();
                    return;
                case 57:
                    position.OBDElements.HybridBatteryPackLife = teltonikaDataPacket.Value.ToUByte1();
                    return;
                case 58:
                    position.OBDElements.EngineOilTemperature = teltonikaDataPacket.Value.ToUByte1();
                    return;
                case 59:
                    position.OBDElements.FuelInjectionTiming = teltonikaDataPacket.Value.ToSShort2();
                    return;
                case 60:
                    position.OBDElements.FuelRate = teltonikaDataPacket.Value.ToUShort2();
                    return;
                case 256:
                    position.OBDElements.VIN = StringUtil.ConvertByteArrayToString(teltonikaDataPacket.Value);
                    return;
                case 281:
                    position.OBDElements.FaultCodes = StringUtil.ConvertByteArrayToString(teltonikaDataPacket.Value);
                    return;
                case 540:
                    position.OBDElements.ThrottlePositionGroup = teltonikaDataPacket.Value.ToUByte1();
                    return;
                case 541:
                    position.OBDElements.CommandedEquivalenceRatio = teltonikaDataPacket.Value.ToUByte1();
                    return;
                case 542:
                    position.OBDElements.IntakeMAP2Bytes = teltonikaDataPacket.Value.ToUShort2();
                    return;
                case 543:
                    position.OBDElements.HybridSystemVoltage = teltonikaDataPacket.Value.ToUShort2();
                    return;
                case 544:
                    position.OBDElements.HybridSystemCurrent = teltonikaDataPacket.Value.ToSShort2();
                    return;
                case 759:
                    position.OBDElements.FuelType = teltonikaDataPacket.Value.ToUByte1();
                    return;

                // OBD OEM elements
                case 389:
                    position.OEMOBDElements.TotalMileage = teltonikaDataPacket.Value.ToUInt4();
                    return;
                case 390:
                    position.OEMOBDElements.FuelLevel = teltonikaDataPacket.Value.ToUInt4();
                    return;
                case 402:
                    position.OEMOBDElements.DistanceUntilService = teltonikaDataPacket.Value.ToUInt4();
                    return;
                case 410:
                    position.OEMOBDElements.BatteryChargeState = teltonikaDataPacket.Value.ToUByte1();
                    return;
                case 411:
                    position.OEMOBDElements.BatteryChargeLevel = teltonikaDataPacket.Value.ToUByte1();
                    return;
                case 412:
                    position.OEMOBDElements.BatteryPowerConsumption = teltonikaDataPacket.Value.ToUShort2();
                    return;
                case 755:
                    position.OEMOBDElements.RemainingDistance = teltonikaDataPacket.Value.ToUShort2();
                    return;
                case 1151:
                    position.OEMOBDElements.BatteryStateOfHealth = teltonikaDataPacket.Value.ToUShort2();
                    return;
                case 1152:
                    position.OEMOBDElements.BatteryTemperature = teltonikaDataPacket.Value.ToSShort2();
                    return;

                default:
                    teltonikaDataPacket.Parsed = false;
                    return;
            }
        }
        catch (Exception)
        {
            teltonikaDataPacket.Parsed = false;
            teltonikaDataPacket.Error = true;
        }
        finally
        {
            string[] array = HexUtil.ConvertByteArrayToHexStringArray(teltonikaDataPacket.Value);

            Array.Reverse(array);
            
            teltonikaDataPacket.Hex = HexUtil.ConvertHexStringArrayToHexString(array);
            teltonikaDataPacket.Value = null;
        }
    }

    private static short GetNumberOfDataPackets(ByteReader input,
        TeltonikaCodecConfiguration teltonikaCodecConfiguration,
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