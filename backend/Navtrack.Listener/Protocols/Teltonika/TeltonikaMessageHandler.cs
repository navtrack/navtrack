using System;
using System.Collections.Generic;
using System.Globalization;
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

        byte noOfLocations = input.DataMessage.ByteReader.GetOne();

        for (byte i = 0; i < noOfLocations; i++)
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
            Position = new PositionElement()
        };

        deviceMessageDocument.Position.Date =
            DateTime.UnixEpoch.AddMilliseconds(input.DataMessage.ByteReader.Get(8).ToSLong8());

        byte priority = input.DataMessage.ByteReader.GetOne();
        
        deviceMessageDocument.MessagePriority = priority switch
        {
            1 => MessagePriority.High,
            2 => MessagePriority.Emergency,
            _ => null
        };
        
        deviceMessageDocument.Position.Longitude = GetCoordinate(input.DataMessage.ByteReader.Get(4));
        deviceMessageDocument.Position.Latitude = GetCoordinate(input.DataMessage.ByteReader.Get(4));
        deviceMessageDocument.Position.Altitude = input.DataMessage.ByteReader.Get(2).ToSShort2();
        deviceMessageDocument.Position.Heading = input.DataMessage.ByteReader.Get(2).ToSShort2();
        deviceMessageDocument.Position.Satellites = input.DataMessage.ByteReader.GetOne();
        deviceMessageDocument.Position.Speed = input.DataMessage.ByteReader.Get(2).ToUShort2();

        ushort eventId = teltonikaCodecConfiguration.MainEventIdLength == 2
            ? input.DataMessage.ByteReader.Get(2).ToUShort2()
            : input.DataMessage.ByteReader.Get(1).ToUByte1();
        
        deviceMessageDocument.AdditionalData[TeltonikaDataIds.EventId.ToString()] = eventId.ToString();

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
                    deviceMessageDocument.AdditionalData[TeltonikaDataIds.DigitalInput1.ToString()] = value.ToBoolean().ToString();
                    return;
                case 11:
                    deviceMessageDocument.AdditionalData[TeltonikaDataIds.ICCID1.ToString()] = value.ToULong8().ToString();
                    return;
                case 14:
                    deviceMessageDocument.AdditionalData[TeltonikaDataIds.ICCID2.ToString()] = value.ToULong8().ToString();
                    return;
                case 15:
                    deviceMessageDocument.AdditionalData[TeltonikaDataIds.EcoScore.ToString()] = (value.ToUShort2() * 0.01).ToString(CultureInfo.InvariantCulture);
                    return;
                case TeltonikaDataIds.TotalOdometer:
                    deviceMessageDocument.Device ??= new DeviceElement();
                    deviceMessageDocument.Device.Odometer = value.ToSInt4();
                    return;
                case TeltonikaDataIds.GsmSignal:
                    deviceMessageDocument.Gsm ??= new GsmElement();
                    deviceMessageDocument.Gsm.SignalLevel = value.ToUByte1();
                    return;
                case 24:
                    deviceMessageDocument.AdditionalData[TeltonikaDataIds.GNSSSpeed.ToString()] = value.ToUShort2().ToString();
                    return;
                case TeltonikaDataIds.ExternalVoltage:
                    deviceMessageDocument.Vehicle ??= new VehicleElement();
                    deviceMessageDocument.Vehicle.Voltage = value.ToSShort2() * 0.001;
                    return;
                case TeltonikaDataIds.BatteryVoltage:
                    deviceMessageDocument.Device ??= new DeviceElement();
                    deviceMessageDocument.Device.BatteryVoltage = value.ToUShort2() * 0.001;
                    return;
                case TeltonikaDataIds.BatteryCurrent:
                    deviceMessageDocument.Device ??= new DeviceElement();
                    deviceMessageDocument.Device.BatteryCurrent = value.ToUShort2() * 0.001;
                    return;
                case 69:
                    deviceMessageDocument.AdditionalData[TeltonikaDataIds.GNSSStatus.ToString()] = value.ToUByte1().ToString();
                    deviceMessageDocument.Position.Valid = value.ToUByte1() == 1;
                    return;
                case TeltonikaDataIds.BatteryLevel:
                    deviceMessageDocument.Device ??= new DeviceElement();
                    deviceMessageDocument.Device.BatteryLevel = value.ToUByte1();
                    return;
                case TeltonikaDataIds.GNSSPDOP:
                    deviceMessageDocument.Position.PDOP = value.ToUShort2() * 0.1;
                    return;
                case TeltonikaDataIds.GNSSHDOP:
                    deviceMessageDocument.Position.HDOP = value.ToUShort2() * 0.1;
                    return;
                case 199:
                    deviceMessageDocument.AdditionalData[TeltonikaDataIds.TripOdometer.ToString()] = value.ToUInt4().ToString();
                    return;
                case 200:
                    deviceMessageDocument.AdditionalData[TeltonikaDataIds.SleepMode.ToString()] = value.ToUByte1().ToString();
                    return;
                case 205:
                    deviceMessageDocument.AdditionalData[TeltonikaDataIds.GsmCellId.ToString()] = value.ToUShort2().ToString();
                    return;
                case 206:
                    deviceMessageDocument.AdditionalData[TeltonikaDataIds.GsmAreaCode.ToString()] = value.ToUShort2().ToString();
                    return;
                case TeltonikaDataIds.Ignition:
                    deviceMessageDocument.Vehicle ??= new VehicleElement();
                    deviceMessageDocument.Vehicle.Ignition = value.ToBoolean();
                    return;
                case 240:
                    deviceMessageDocument.AdditionalData[TeltonikaDataIds.Movement.ToString()] = value.ToBoolean().ToString();
                    return;
                case TeltonikaDataIds.ActiveGSMOperator:
                    string homeNetworkIdentity = $"{value.ToUInt4()}";

                    if (homeNetworkIdentity.Length > 4)
                    {
                        deviceMessageDocument.Gsm ??= new GsmElement();
                        deviceMessageDocument.Gsm.CellGlobalIdentity ??= new CellGlobalIdentityElement();
                        deviceMessageDocument.Gsm.CellGlobalIdentity.MobileCountryCode = homeNetworkIdentity[..3];
                        deviceMessageDocument.Gsm.CellGlobalIdentity.MobileNetworkCode = homeNetworkIdentity[3..];
                    }

                    return;
                case 636:
                    deviceMessageDocument.AdditionalData[TeltonikaDataIds.UMTSLTECellID.ToString()] = value.ToUInt4().ToString();
                    return;

                // Event I/O elements
                case 250:
                    deviceMessageDocument.AdditionalData[TeltonikaDataIds.EventTrip.ToString()] = value.ToUByte1().ToString();
                    return;

                // OBD elements
                case 30:
                    deviceMessageDocument.AdditionalData[TeltonikaDataIds.ObdDtcCount.ToString()] = value.ToUByte1().ToString();
                    return;
                case 31:
                    deviceMessageDocument.AdditionalData[TeltonikaDataIds.ObdEngineLoad.ToString()] = value.ToUByte1().ToString();
                    return;
                case 32:
                    deviceMessageDocument.AdditionalData[TeltonikaDataIds.ObdCoolantTemperature.ToString()] = value.ToSByte1().ToString();
                    return;
                case 33:
                    deviceMessageDocument.AdditionalData[TeltonikaDataIds.ObdShortTermFuelTrim.ToString()] = value.ToSByte1().ToString();
                    return;
                case 34:
                    deviceMessageDocument.AdditionalData[TeltonikaDataIds.ObdFuelPressure.ToString()] = value.ToULong8().ToString();
                    return;
                case 35:
                    deviceMessageDocument.AdditionalData[TeltonikaDataIds.ObdIntakeManifoldAbsolutPressure.ToString()] = value.ToUByte1().ToString();
                    return;
                case 36:
                    deviceMessageDocument.AdditionalData[TeltonikaDataIds.ObdEngineRpm.ToString()] = value.ToUShort2().ToString();
                    return;
                case 37:
                    deviceMessageDocument.AdditionalData[TeltonikaDataIds.ObdSpeed.ToString()] = value.ToUByte1().ToString();
                    return;
                case 38:
                    deviceMessageDocument.AdditionalData[TeltonikaDataIds.ObdTimingAdvance.ToString()] = value.ToSByte1().ToString();
                    return;
                case 39:
                    deviceMessageDocument.AdditionalData[TeltonikaDataIds.ObdIntakeAirTemperature.ToString()] = value.ToSByte1().ToString();
                    return;
                case 40:
                    deviceMessageDocument.AdditionalData[TeltonikaDataIds.ObdMAFAirFlowRate.ToString()] = value.ToUShort2().ToString();
                    return;
                case 41:
                    deviceMessageDocument.AdditionalData[TeltonikaDataIds.ObdThrottlePosition.ToString()] = value.ToUByte1().ToString();
                    return;
                case 42:
                    deviceMessageDocument.AdditionalData[TeltonikaDataIds.ObdRuntimeSinceEngineStart.ToString()] = value.ToUShort2().ToString();
                    return;
                case 43:
                    deviceMessageDocument.AdditionalData[TeltonikaDataIds.ObdDistanceTraveledMILOn.ToString()] = value.ToUShort2().ToString();
                    return;
                case 44:
                    deviceMessageDocument.AdditionalData[TeltonikaDataIds.ObdRelativeFuelRailPressure.ToString()] = (value.ToUShort2() * 0.1).ToString(CultureInfo.InvariantCulture);
                    return;
                case 45:
                    deviceMessageDocument.AdditionalData[TeltonikaDataIds.ObdDirectFuelRailPressure.ToString()] = (value.ToUShort2() * 10).ToString();
                    return;
                case 46:
                    deviceMessageDocument.AdditionalData[TeltonikaDataIds.ObdCommandedEGR.ToString()] = value.ToUByte1().ToString();
                    return;
                case 47:
                    deviceMessageDocument.AdditionalData[TeltonikaDataIds.ObdEGRError.ToString()] = value.ToSByte1().ToString();
                    return;
                case 48:
                    deviceMessageDocument.AdditionalData[TeltonikaDataIds.ObdFuelLevel.ToString()] = value.ToUByte1().ToString();
                    return;
                case 49:
                    deviceMessageDocument.AdditionalData[TeltonikaDataIds.ObdDistanceSinceCodesClear.ToString()] = value.ToUShort2().ToString();
                    return;
                case 50:
                    deviceMessageDocument.AdditionalData[TeltonikaDataIds.ObdBarometicPressure.ToString()] = value.ToUByte1().ToString();
                    return;
                case 51:
                    deviceMessageDocument.AdditionalData[TeltonikaDataIds.ObdControlModuleVoltage.ToString()] = value.ToUShort2().ToString();
                    return;
                case 52:
                    deviceMessageDocument.AdditionalData[TeltonikaDataIds.ObdAbsoluteLoadValue.ToString()] = value.ToUShort2().ToString();
                    return;
                case 53:
                    deviceMessageDocument.AdditionalData[TeltonikaDataIds.ObdAmbientAirTemperature.ToString()] = value.ToSByte1().ToString();
                    return;
                case 54:
                    deviceMessageDocument.AdditionalData[TeltonikaDataIds.ObdTimeRunWithMILOn.ToString()] = value.ToUShort2().ToString();
                    return;
                case 55:
                    deviceMessageDocument.AdditionalData[TeltonikaDataIds.ObdTimeSinceCodesCleared.ToString()] = value.ToUShort2().ToString();
                    return;
                case 56:
                    deviceMessageDocument.AdditionalData[TeltonikaDataIds.ObdAbsoluteFuelRailPressure.ToString()] = value.ToUShort2().ToString();
                    return;
                case 57:
                    deviceMessageDocument.AdditionalData[TeltonikaDataIds.ObdHybridBatteryPackLife.ToString()] = value.ToUByte1().ToString();
                    return;
                case 58:
                    deviceMessageDocument.AdditionalData[TeltonikaDataIds.ObdEngineOilTemperature.ToString()] = value.ToUByte1().ToString();
                    return;
                case 59:
                    deviceMessageDocument.AdditionalData[TeltonikaDataIds.ObdFuelInjectionTiming.ToString()] = (value.ToSShort2() * 0.01).ToString(CultureInfo.InvariantCulture);
                    return;
                case 60:
                    deviceMessageDocument.AdditionalData[TeltonikaDataIds.ObdFuelRate.ToString()] = value.ToUShort2().ToString();
                    return;
                case 256:
                    deviceMessageDocument.AdditionalData[TeltonikaDataIds.ObdVIN.ToString()] = StringUtil.ConvertByteArrayToString(value);
                    return;
                case 281:
                    deviceMessageDocument.AdditionalData[TeltonikaDataIds.ObdFaultCodes.ToString()] =
                        StringUtil.ConvertByteArrayToString(value);
                    return;
                case 540:
                    deviceMessageDocument.AdditionalData[TeltonikaDataIds.ObdThrottlePositionGroup.ToString()] = value.ToUByte1().ToString();
                    return;
                case 541:
                    deviceMessageDocument.AdditionalData[TeltonikaDataIds.ObdCommandedEquivalenceRatio.ToString()] = value.ToUByte1().ToString();
                    return;
                case 542:
                    deviceMessageDocument.AdditionalData[TeltonikaDataIds.ObdIntakeMAP2Bytes.ToString()] = value.ToUShort2().ToString();
                    return;
                case 543:
                    deviceMessageDocument.AdditionalData[TeltonikaDataIds.ObdHybridSystemVoltage.ToString()] = value.ToUShort2().ToString();
                    return;
                case 544:
                    deviceMessageDocument.AdditionalData[TeltonikaDataIds.ObdHybridSystemCurrent.ToString()] = value.ToSShort2().ToString();
                    return;
                case 759:
                    deviceMessageDocument.AdditionalData[TeltonikaDataIds.ObdFuelType.ToString()] = value.ToUByte1().ToString();
                    return;

                // OBD OEM elements
                case TeltonikaDataIds.ObdOemMileage:
                    deviceMessageDocument.Vehicle ??= new VehicleElement();
                    deviceMessageDocument.Vehicle.Odometer = value.ToUInt4();
                    return;
                case 390:
                    deviceMessageDocument.AdditionalData[TeltonikaDataIds.ObdOemFuelLevel.ToString()] = (value.ToUInt4() * 0.1).ToString(CultureInfo.InvariantCulture);
                    return;
                case 402:
                    deviceMessageDocument.AdditionalData[TeltonikaDataIds.ObdOemDistanceUntilService.ToString()] = value.ToUInt4().ToString();
                    return;
                case 410:
                    deviceMessageDocument.AdditionalData[TeltonikaDataIds.ObdOemBatteryChargeState.ToString()] = value.ToUByte1().ToString();
                    return;
                case 411:
                    deviceMessageDocument.AdditionalData[TeltonikaDataIds.ObdOemBatteryLevel.ToString()] = value.ToUByte1().ToString();
                    return;
                case 412:
                    deviceMessageDocument.AdditionalData[TeltonikaDataIds.ObdOemBatteryPowerConsumption.ToString()] = value.ToUShort2().ToString();
                    return;
                case 755:
                    deviceMessageDocument.AdditionalData[TeltonikaDataIds.ObdOemRemainingDistance.ToString()] = value.ToUShort2().ToString();
                    return;
                case 1151:
                    deviceMessageDocument.AdditionalData[TeltonikaDataIds.ObdOemBatteryStateOfHealth.ToString()] = value.ToUShort2().ToString();
                    return;
                case 1152:
                    deviceMessageDocument.AdditionalData[TeltonikaDataIds.ObdOemBatteryTemperature.ToString()] = value.ToSShort2().ToString();
                    return;

                // CAN adapters
                case 90:
                    deviceMessageDocument.AdditionalData[TeltonikaDataIds.CanDoorStatus.ToString()] = value.ToSShort2().ToString();
                    return;
                case 100:
                    deviceMessageDocument.AdditionalData[TeltonikaDataIds.CanProgramNumber.ToString()] = value.ToUInt4().ToString();
                    return;
                case 123:
                    deviceMessageDocument.AdditionalData[TeltonikaDataIds.CanControlStateFlags.ToString()] = value.ToUInt4().ToString();
                    return;
                case 124:
                    deviceMessageDocument.AdditionalData[TeltonikaDataIds.CanAgriculturalMachineryFlags.ToString()] = value.ToULong8().ToString();
                    return;
                case 132:
                    deviceMessageDocument.AdditionalData[TeltonikaDataIds.CanSecurityStateFlags.ToString()] = value.ToULong8().ToString();
                    return;
                case 232:
                    deviceMessageDocument.AdditionalData[TeltonikaDataIds.CanCNGStatus.ToString()] = value.ToUByte1().ToString();
                    return;
                case 517:
                    deviceMessageDocument.AdditionalData[TeltonikaDataIds.CanSecurityStateFlagsP4.ToString()] = value.ToULong8().ToString();
                    return;
                default:

                    string[] array = HexUtil.ConvertByteArrayToHexStringArray(value);
                    Array.Reverse(array);

                    deviceMessageDocument.AdditionalDataUnhandled[id.ToString()] = HexUtil.ConvertHexStringArrayToHexString(array);

                    return;
            }
        }
        catch (Exception)
        {
            string[] array = HexUtil.ConvertByteArrayToHexStringArray(value);
            Array.Reverse(array);

            deviceMessageDocument.AdditionalDataUnhandled[id.ToString()] =
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