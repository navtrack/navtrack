using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Navtrack.Database.Model.Devices;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

#pragma warning disable CS8602 // Dereference of a possibly null reference.

namespace Navtrack.Listener.Protocols.Teltonika;

[Service(typeof(ICustomMessageHandler<TeltonikaProtocol>))]
public class TeltonikaMessageHandler : BaseMessageHandler<TeltonikaProtocol>
{
    public override IEnumerable<DeviceMessageEntity>? ParseRange(MessageInput input)
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

        List<DeviceMessageEntity> positions = GetPositions(input);

        if (positions.Count != 0)
        {
            string reply = positions.Count.ToString("X8");

            input.NetworkStream.Write(HexUtil.ConvertHexStringToByteArray(reply));
        }

        return positions;
    }

    private static List<DeviceMessageEntity> GetPositions(MessageInput input)
    {
        List<DeviceMessageEntity> messages = [];

        TeltonikaCodecConfiguration? codecConfiguration = GetCodec(input);

        if (codecConfiguration == null)
        {
            return [];
        }

        byte noOfLocations = input.DataMessage.ByteReader.GetOne();

        for (byte i = 0; i < noOfLocations; i++)
        {
            DeviceMessageEntity deviceMessage = GetPosition(input, codecConfiguration);

            messages.Add(deviceMessage);
        }

        return messages;
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

    private static DeviceMessageEntity GetPosition(MessageInput input,
        TeltonikaCodecConfiguration? teltonikaCodecConfiguration)
    {
        DeviceMessageEntity deviceMessage = new();
        deviceMessage.AdditionalDataDic = new Dictionary<string, string>();
        

        deviceMessage.Date =
            DateTime.UnixEpoch.AddMilliseconds(input.DataMessage.ByteReader.Get(8).ToSLong8());

        byte priority = input.DataMessage.ByteReader.GetOne();
        
        deviceMessage.MessagePriority = priority switch
        {
            1 => MessagePriority.High,
            2 => MessagePriority.Emergency,
            _ => null
        };
        
        deviceMessage.Longitude = GetCoordinate(input.DataMessage.ByteReader.Get(4));
        deviceMessage.Latitude = GetCoordinate(input.DataMessage.ByteReader.Get(4));
        deviceMessage.Altitude = input.DataMessage.ByteReader.Get(2).ToSShort2();
        deviceMessage.Heading = input.DataMessage.ByteReader.Get(2).ToSShort2();
        deviceMessage.Satellites = input.DataMessage.ByteReader.GetOne();
        deviceMessage.Speed = input.DataMessage.ByteReader.Get(2).ToSShort2();

        ushort eventId = teltonikaCodecConfiguration.MainEventIdLength == 2
            ? input.DataMessage.ByteReader.Get(2).ToUShort2()
            : input.DataMessage.ByteReader.Get(1).ToUByte1();
        
        deviceMessage.AdditionalDataDic[TeltonikaDataIds.EventId.ToString()] = eventId.ToString();

        if (teltonikaCodecConfiguration.HasGenerationType)
        {
            byte generationType = input.DataMessage.ByteReader.GetOne();
        }

        byte[] dataPacketsCount = input.DataMessage.ByteReader.Get(teltonikaCodecConfiguration.DataPacketCountBytes);

        MapDataPackets(deviceMessage, input.DataMessage.ByteReader, teltonikaCodecConfiguration,
            1); // 1 byte events
        MapDataPackets(deviceMessage, input.DataMessage.ByteReader, teltonikaCodecConfiguration,
            2); // 2 bytes events
        MapDataPackets(deviceMessage, input.DataMessage.ByteReader, teltonikaCodecConfiguration,
            4); // 4 bytes events
        MapDataPackets(deviceMessage, input.DataMessage.ByteReader, teltonikaCodecConfiguration,
            8); // 8 bytes events
        MapDataPackets(deviceMessage, input.DataMessage.ByteReader,
            teltonikaCodecConfiguration); // variable bytes events

        return deviceMessage;
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

    private static void MapDataPackets(DeviceMessageEntity deviceMessage, ByteReader input,
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


            MapDataPacket(id, value, deviceMessage);
        }
    }

    private static void MapDataPacket(short id, byte[] value, DeviceMessageEntity deviceMessageDocument)
    {
        try
        {
            switch (id)
            {
                // Permanent I/O elements
                case 1:
                    deviceMessageDocument.AdditionalDataDic[TeltonikaDataIds.DigitalInput1.ToString()] = value.ToBoolean().ToString();
                    return;
                case 11:
                    deviceMessageDocument.AdditionalDataDic[TeltonikaDataIds.ICCID1.ToString()] = value.ToULong8().ToString();
                    return;
                case 14:
                    deviceMessageDocument.AdditionalDataDic[TeltonikaDataIds.ICCID2.ToString()] = value.ToULong8().ToString();
                    return;
                case 15:
                    deviceMessageDocument.AdditionalDataDic[TeltonikaDataIds.EcoScore.ToString()] = (value.ToUShort2() * 0.01).ToString(CultureInfo.InvariantCulture);
                    return;
                case TeltonikaDataIds.TotalOdometer:
                    deviceMessageDocument.DeviceOdometer = value.ToSInt4();
                    return;
                case TeltonikaDataIds.GsmSignal:
                    deviceMessageDocument.GSMSignalLevel = value.ToUByte1();
                    return;
                case 24:
                    deviceMessageDocument.AdditionalDataDic[TeltonikaDataIds.GNSSSpeed.ToString()] = value.ToUShort2().ToString();
                    return;
                case TeltonikaDataIds.ExternalVoltage:
                    deviceMessageDocument.VehicleVoltage = value.ToSShort2() * 0.001f;
                    return;
                case TeltonikaDataIds.BatteryVoltage:
                    deviceMessageDocument.DeviceBatteryVoltage = value.ToUShort2() * 0.001f;
                    return;
                case TeltonikaDataIds.BatteryCurrent:
                    deviceMessageDocument.DeviceBatteryCurrent = value.ToUShort2() * 0.001f;
                    return;
                case 69:
                    deviceMessageDocument.AdditionalDataDic[TeltonikaDataIds.GNSSStatus.ToString()] = value.ToUByte1().ToString();
                    deviceMessageDocument.Valid = value.ToUByte1() == 1;
                    return;
                case TeltonikaDataIds.BatteryLevel:
                    deviceMessageDocument.DeviceBatteryLevel = value.ToUByte1();
                    return;
                case TeltonikaDataIds.GNSSPDOP:
                    deviceMessageDocument.PDOP = value.ToUShort2() * 0.1f;
                    return;
                case TeltonikaDataIds.GNSSHDOP:
                    deviceMessageDocument.HDOP = value.ToUShort2() * 0.1f;
                    return;
                case 199:
                    deviceMessageDocument.AdditionalDataDic[TeltonikaDataIds.TripOdometer.ToString()] = value.ToUInt4().ToString();
                    return;
                case 200:
                    deviceMessageDocument.AdditionalDataDic[TeltonikaDataIds.SleepMode.ToString()] = value.ToUByte1().ToString();
                    return;
                case 205:
                    deviceMessageDocument.AdditionalDataDic[TeltonikaDataIds.GsmCellId.ToString()] = value.ToUShort2().ToString();
                    return;
                case 206:
                    deviceMessageDocument.AdditionalDataDic[TeltonikaDataIds.GsmAreaCode.ToString()] = value.ToUShort2().ToString();
                    return;
                case TeltonikaDataIds.Ignition:
                    deviceMessageDocument.VehicleIgnition = value.ToBoolean();
                    return;
                case 240:
                    deviceMessageDocument.AdditionalDataDic[TeltonikaDataIds.Movement.ToString()] = value.ToBoolean().ToString();
                    return;
                case TeltonikaDataIds.ActiveGSMOperator:
                    string homeNetworkIdentity = $"{value.ToUInt4()}";

                    if (homeNetworkIdentity.Length > 4)
                    {
                        deviceMessageDocument.GSMMobileCountryCode = homeNetworkIdentity[..3];
                        deviceMessageDocument.GSMMobileNetworkCode = homeNetworkIdentity[3..];
                    }

                    return;
                case 636:
                    deviceMessageDocument.AdditionalDataDic[TeltonikaDataIds.UMTSLTECellID.ToString()] = value.ToUInt4().ToString();
                    return;

                // Event I/O elements
                case 250:
                    deviceMessageDocument.AdditionalDataDic[TeltonikaDataIds.EventTrip.ToString()] = value.ToUByte1().ToString();
                    return;
                case TeltonikaDataIds.IgnitionOnCounter:
                    deviceMessageDocument.VehicleIgnitionDuration = value.ToSInt4();
                    return;

                // OBD elements
                case 30:
                    deviceMessageDocument.AdditionalDataDic[TeltonikaDataIds.ObdDtcCount.ToString()] = value.ToUByte1().ToString();
                    return;
                case 31:
                    deviceMessageDocument.AdditionalDataDic[TeltonikaDataIds.ObdEngineLoad.ToString()] = value.ToUByte1().ToString();
                    return;
                case 32:
                    deviceMessageDocument.AdditionalDataDic[TeltonikaDataIds.ObdCoolantTemperature.ToString()] = value.ToSByte1().ToString();
                    return;
                case 33:
                    deviceMessageDocument.AdditionalDataDic[TeltonikaDataIds.ObdShortTermFuelTrim.ToString()] = value.ToSByte1().ToString();
                    return;
                case 34:
                    deviceMessageDocument.AdditionalDataDic[TeltonikaDataIds.ObdFuelPressure.ToString()] = value.ToULong8().ToString();
                    return;
                case 35:
                    deviceMessageDocument.AdditionalDataDic[TeltonikaDataIds.ObdIntakeManifoldAbsolutPressure.ToString()] = value.ToUByte1().ToString();
                    return;
                case 36:
                    deviceMessageDocument.AdditionalDataDic[TeltonikaDataIds.ObdEngineRpm.ToString()] = value.ToUShort2().ToString();
                    return;
                case 37:
                    deviceMessageDocument.AdditionalDataDic[TeltonikaDataIds.ObdSpeed.ToString()] = value.ToUByte1().ToString();
                    return;
                case 38:
                    deviceMessageDocument.AdditionalDataDic[TeltonikaDataIds.ObdTimingAdvance.ToString()] = value.ToSByte1().ToString();
                    return;
                case 39:
                    deviceMessageDocument.AdditionalDataDic[TeltonikaDataIds.ObdIntakeAirTemperature.ToString()] = value.ToSByte1().ToString();
                    return;
                case 40:
                    deviceMessageDocument.AdditionalDataDic[TeltonikaDataIds.ObdMAFAirFlowRate.ToString()] = value.ToUShort2().ToString();
                    return;
                case 41:
                    deviceMessageDocument.AdditionalDataDic[TeltonikaDataIds.ObdThrottlePosition.ToString()] = value.ToUByte1().ToString();
                    return;
                case 42:
                    deviceMessageDocument.AdditionalDataDic[TeltonikaDataIds.ObdRuntimeSinceEngineStart.ToString()] = value.ToUShort2().ToString();
                    return;
                case 43:
                    deviceMessageDocument.AdditionalDataDic[TeltonikaDataIds.ObdDistanceTraveledMILOn.ToString()] = value.ToUShort2().ToString();
                    return;
                case 44:
                    deviceMessageDocument.AdditionalDataDic[TeltonikaDataIds.ObdRelativeFuelRailPressure.ToString()] = (value.ToUShort2() * 0.1).ToString(CultureInfo.InvariantCulture);
                    return;
                case 45:
                    deviceMessageDocument.AdditionalDataDic[TeltonikaDataIds.ObdDirectFuelRailPressure.ToString()] = (value.ToUShort2() * 10).ToString();
                    return;
                case 46:
                    deviceMessageDocument.AdditionalDataDic[TeltonikaDataIds.ObdCommandedEGR.ToString()] = value.ToUByte1().ToString();
                    return;
                case 47:
                    deviceMessageDocument.AdditionalDataDic[TeltonikaDataIds.ObdEGRError.ToString()] = value.ToSByte1().ToString();
                    return;
                case 48:
                    deviceMessageDocument.AdditionalDataDic[TeltonikaDataIds.ObdFuelLevel.ToString()] = value.ToUByte1().ToString();
                    return;
                case 49:
                    deviceMessageDocument.AdditionalDataDic[TeltonikaDataIds.ObdDistanceSinceCodesClear.ToString()] = value.ToUShort2().ToString();
                    return;
                case 50:
                    deviceMessageDocument.AdditionalDataDic[TeltonikaDataIds.ObdBarometicPressure.ToString()] = value.ToUByte1().ToString();
                    return;
                case 51:
                    deviceMessageDocument.AdditionalDataDic[TeltonikaDataIds.ObdControlModuleVoltage.ToString()] = value.ToUShort2().ToString();
                    return;
                case 52:
                    deviceMessageDocument.AdditionalDataDic[TeltonikaDataIds.ObdAbsoluteLoadValue.ToString()] = value.ToUShort2().ToString();
                    return;
                case 53:
                    deviceMessageDocument.AdditionalDataDic[TeltonikaDataIds.ObdAmbientAirTemperature.ToString()] = value.ToSByte1().ToString();
                    return;
                case 54:
                    deviceMessageDocument.AdditionalDataDic[TeltonikaDataIds.ObdTimeRunWithMILOn.ToString()] = value.ToUShort2().ToString();
                    return;
                case 55:
                    deviceMessageDocument.AdditionalDataDic[TeltonikaDataIds.ObdTimeSinceCodesCleared.ToString()] = value.ToUShort2().ToString();
                    return;
                case 56:
                    deviceMessageDocument.AdditionalDataDic[TeltonikaDataIds.ObdAbsoluteFuelRailPressure.ToString()] = value.ToUShort2().ToString();
                    return;
                case 57:
                    deviceMessageDocument.AdditionalDataDic[TeltonikaDataIds.ObdHybridBatteryPackLife.ToString()] = value.ToUByte1().ToString();
                    return;
                case 58:
                    deviceMessageDocument.AdditionalDataDic[TeltonikaDataIds.ObdEngineOilTemperature.ToString()] = value.ToUByte1().ToString();
                    return;
                case 59:
                    deviceMessageDocument.AdditionalDataDic[TeltonikaDataIds.ObdFuelInjectionTiming.ToString()] = (value.ToSShort2() * 0.01).ToString(CultureInfo.InvariantCulture);
                    return;
                case 60:
                    deviceMessageDocument.AdditionalDataDic[TeltonikaDataIds.ObdFuelRate.ToString()] = value.ToUShort2().ToString();
                    return;
                case 256:
                    deviceMessageDocument.AdditionalDataDic[TeltonikaDataIds.ObdVIN.ToString()] = StringUtil.ConvertByteArrayToString(value);
                    return;
                case 281:
                    deviceMessageDocument.AdditionalDataDic[TeltonikaDataIds.ObdFaultCodes.ToString()] =
                        StringUtil.ConvertByteArrayToString(value);
                    return;
                case 540:
                    deviceMessageDocument.AdditionalDataDic[TeltonikaDataIds.ObdThrottlePositionGroup.ToString()] = value.ToUByte1().ToString();
                    return;
                case 541:
                    deviceMessageDocument.AdditionalDataDic[TeltonikaDataIds.ObdCommandedEquivalenceRatio.ToString()] = value.ToUByte1().ToString();
                    return;
                case 542:
                    deviceMessageDocument.AdditionalDataDic[TeltonikaDataIds.ObdIntakeMAP2Bytes.ToString()] = value.ToUShort2().ToString();
                    return;
                case 543:
                    deviceMessageDocument.AdditionalDataDic[TeltonikaDataIds.ObdHybridSystemVoltage.ToString()] = value.ToUShort2().ToString();
                    return;
                case 544:
                    deviceMessageDocument.AdditionalDataDic[TeltonikaDataIds.ObdHybridSystemCurrent.ToString()] = value.ToSShort2().ToString();
                    return;
                case 759:
                    deviceMessageDocument.AdditionalDataDic[TeltonikaDataIds.ObdFuelType.ToString()] = value.ToUByte1().ToString();
                    return;

                // OBD OEM elements
                case TeltonikaDataIds.ObdOemMileage:
                    deviceMessageDocument.VehicleOdometer = value.ToSInt4();
                    return;
                case 390:
                    deviceMessageDocument.AdditionalDataDic[TeltonikaDataIds.ObdOemFuelLevel.ToString()] = (value.ToUInt4() * 0.1).ToString(CultureInfo.InvariantCulture);
                    return;
                case 402:
                    deviceMessageDocument.AdditionalDataDic[TeltonikaDataIds.ObdOemDistanceUntilService.ToString()] = value.ToUInt4().ToString();
                    return;
                case 410:
                    deviceMessageDocument.AdditionalDataDic[TeltonikaDataIds.ObdOemBatteryChargeState.ToString()] = value.ToUByte1().ToString();
                    return;
                case 411:
                    deviceMessageDocument.AdditionalDataDic[TeltonikaDataIds.ObdOemBatteryLevel.ToString()] = value.ToUByte1().ToString();
                    return;
                case 412:
                    deviceMessageDocument.AdditionalDataDic[TeltonikaDataIds.ObdOemBatteryPowerConsumption.ToString()] = value.ToUShort2().ToString();
                    return;
                case 755:
                    deviceMessageDocument.AdditionalDataDic[TeltonikaDataIds.ObdOemRemainingDistance.ToString()] = value.ToUShort2().ToString();
                    return;
                case 1151:
                    deviceMessageDocument.AdditionalDataDic[TeltonikaDataIds.ObdOemBatteryStateOfHealth.ToString()] = value.ToUShort2().ToString();
                    return;
                case 1152:
                    deviceMessageDocument.AdditionalDataDic[TeltonikaDataIds.ObdOemBatteryTemperature.ToString()] = value.ToSShort2().ToString();
                    return;

                // CAN adapters
                case 90:
                    deviceMessageDocument.AdditionalDataDic[TeltonikaDataIds.CanDoorStatus.ToString()] = value.ToSShort2().ToString();
                    return;
                case 100:
                    deviceMessageDocument.AdditionalDataDic[TeltonikaDataIds.CanProgramNumber.ToString()] = value.ToUInt4().ToString();
                    return;
                case TeltonikaDataIds.CanFuelConsumedCounted:
                    deviceMessageDocument.VehicleFuelConsumption = value.ToSInt4() * 0.1f;
                    return;
                case 123:
                    deviceMessageDocument.AdditionalDataDic[TeltonikaDataIds.CanControlStateFlags.ToString()] = value.ToUInt4().ToString();
                    return;
                case 124:
                    deviceMessageDocument.AdditionalDataDic[TeltonikaDataIds.CanAgriculturalMachineryFlags.ToString()] = value.ToULong8().ToString();
                    return;
                case 132:
                    deviceMessageDocument.AdditionalDataDic[TeltonikaDataIds.CanSecurityStateFlags.ToString()] = value.ToULong8().ToString();
                    return;
                case 232:
                    deviceMessageDocument.AdditionalDataDic[TeltonikaDataIds.CanCNGStatus.ToString()] = value.ToUByte1().ToString();
                    return;
                case 517:
                    deviceMessageDocument.AdditionalDataDic[TeltonikaDataIds.CanSecurityStateFlagsP4.ToString()] = value.ToULong8().ToString();
                    return;
                default:

                    string[] array = HexUtil.ConvertByteArrayToHexStringArray(value);
                    Array.Reverse(array);

                    // TODO
                    // deviceMessageDocument.AdditionalDataUnhandled[id.ToString()] = HexUtil.ConvertHexStringArrayToHexString(array);

                    return;
            }
        }
        catch (Exception)
        {
            string[] array = HexUtil.ConvertByteArrayToHexStringArray(value);
            Array.Reverse(array);

            // TODO
            // deviceMessageDocument.AdditionalDataException[id.ToString()] =
            //     HexUtil.ConvertHexStringArrayToHexString(array);
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