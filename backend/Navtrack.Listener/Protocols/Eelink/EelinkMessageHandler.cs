using System;
using System.Collections.Generic;
using Navtrack.DataAccess.Model.Devices.Messages;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;
using static System.String;

namespace Navtrack.Listener.Protocols.Eelink;

[Service(typeof(ICustomMessageHandler<EelinkProtocol>))]
public class EelinkMessageHandler : BaseMessageHandler<EelinkProtocol>
{
    public override DeviceMessageDocument Parse(MessageInput input)
    {
        PackageIdentifier packageIdentifier = (PackageIdentifier)input.DataMessage.Bytes[2];

        Dictionary<PackageIdentifier, Func<DeviceMessageDocument>> dictionary = new()
        {
            {
                PackageIdentifier.Login, () =>
                {
                    HandleLoginPackage(input);

                    return null;
                }
            },
            {
                PackageIdentifier.Heartbeat, () =>
                {
                    SendAcknowledge(input);

                    return null;
                }
            },
            { PackageIdentifier.Location, () => HandleLocationForV20(input) },
            { PackageIdentifier.Warning, () => HandleLocationForV20(input) },
            { PackageIdentifier.Report, () => HandleLocationForV20(input) },
            {
                PackageIdentifier.Message, () =>
                {
                    DeviceMessageDocument deviceMessageDocument = GetLocationV20(input);

                    string number =
                        HexUtil.ConvertHexStringArrayToHexString(
                            HexUtil.ConvertByteArrayToHexStringArray(input.DataMessage.ByteReader.Get(21)));

                    SendAcknowledge(input, number);

                    return deviceMessageDocument;
                }
            },
            {
                PackageIdentifier.ParamSet, () =>
                {
                    SendAcknowledge(input, "00");

                    return null;
                }
            },
            { PackageIdentifier.GPSOld, () => HandleMessageForV18(input, false) },
            { PackageIdentifier.AlarmOld, () => HandleMessageForV18(input) },
            { PackageIdentifier.TerminalStateOld, () => HandleMessageForV18(input) }
        };

        return dictionary.ContainsKey(packageIdentifier) ? dictionary[packageIdentifier].Invoke() : null;
    }

    private static DeviceMessageDocument HandleLocationForV20(MessageInput input)
    {
        DeviceMessageDocument deviceMessageDocument = GetLocationV20(input);

        SendAcknowledge(input);

        return deviceMessageDocument;
    }

    private static DeviceMessageDocument HandleMessageForV18(MessageInput input, bool sendAcknowledge = true)
    {
        DeviceMessageDocument deviceMessageDocument = GetLocationV18(input, 7);

        if (sendAcknowledge)
        {
            SendAcknowledge(input);
        }

        return deviceMessageDocument;
    }

    private static DeviceMessageDocument GetLocationV18(MessageInput input, int startIndex)
    {
        DeviceMessageDocument deviceMessageDocument = new()
        {
            // Device = input.ConnectionContext.Device,
            Position = new PositionElement()
        };

        input.DataMessage.ByteReader.Skip(startIndex);
        deviceMessageDocument.Position.Date =
            DateTime.UnixEpoch.AddSeconds(input.DataMessage.ByteReader.Get(4).ToSInt4());
        deviceMessageDocument.Position.Latitude = input.DataMessage.ByteReader.Get(4).ToSInt4() / 1800000.0;
        deviceMessageDocument.Position.Longitude = input.DataMessage.ByteReader.Get(4).ToSInt4() / 1800000.0;
        deviceMessageDocument.Position.Speed = input.DataMessage.ByteReader.Get(2).ToSShort2();
        deviceMessageDocument.Position.Heading = input.DataMessage.ByteReader.Get(2).ToSShort2();
        deviceMessageDocument.Gsm = new GsmElement
        {
            MobileCountryCode = input.DataMessage.ByteReader.Get(2).ToSShort2().ToString(),
            MobileNetworkCode = input.DataMessage.ByteReader.Get(2).ToSShort2().ToString(),
            LocationAreaCode = input.DataMessage.ByteReader.Get(2).ToSShort2().ToString(),
            CellId = GetCellId(input.DataMessage.ByteReader)
        };

        string status = Convert.ToString(input.DataMessage.ByteReader.GetOne(), 2).PadLeft(8, '0');
        deviceMessageDocument.Position.Valid = status[^1] == '1';

        return deviceMessageDocument;
    }

    private static int GetCellId(ByteReader dataMessageByteReader)
    {
        byte[] array =
            [0x00, dataMessageByteReader.GetOne(), dataMessageByteReader.GetOne(), dataMessageByteReader.GetOne()];

        return array.ToSInt4();
    }

    private static DeviceMessageDocument GetLocationV20(MessageInput input)
    {
        const int locationStartIndex = 7;

        DeviceMessageDocument deviceMessageDocument = new()
        {
            // Device = input.ConnectionContext.Device
            Position = new PositionElement()
        };

        input.DataMessage.ByteReader.Skip(locationStartIndex);
        deviceMessageDocument.Position.Date =
            DateTime.UnixEpoch.AddSeconds(input.DataMessage.ByteReader.Get(4).ToSInt4());
        string mask = Convert.ToString(input.DataMessage.ByteReader.GetOne(), 2).PadLeft(8, '0');

        if (mask[^1] == '1')
        {
            deviceMessageDocument.Position.Latitude = input.DataMessage.ByteReader.Get(4).ToSInt4() / 1800000.0;
            deviceMessageDocument.Position.Longitude = input.DataMessage.ByteReader.Get(4).ToSInt4() / 1800000.0;
            deviceMessageDocument.Position.Altitude = input.DataMessage.ByteReader.Get(2).ToSShort2();
            deviceMessageDocument.Position.Speed = input.DataMessage.ByteReader.Get(2).ToSShort2();
            deviceMessageDocument.Position.Heading = input.DataMessage.ByteReader.Get(2).ToSShort2();
            deviceMessageDocument.Position.Satellites = Convert.ToInt16(input.DataMessage.ByteReader.GetOne());
        }

        if (mask[^2] == '1')
        {
            deviceMessageDocument.Gsm = new GsmElement
            {
                MobileCountryCode = input.DataMessage.ByteReader.Get(2).ToSShort2().ToString(),
                MobileNetworkCode = input.DataMessage.ByteReader.Get(2).ToSShort2().ToString(),
                LocationAreaCode = input.DataMessage.ByteReader.Get(2).ToSShort2().ToString(),
                CellId = input.DataMessage.ByteReader.Get(4).ToSInt4(),
                SignalStrength = input.DataMessage.ByteReader.GetOne()
            };
        }

        return deviceMessageDocument;
    }

    private static void HandleLoginPackage(MessageInput input)
    {
        input.ConnectionContext.SetDevice(input.DataMessage.Hex[7..15].StringJoin().TrimStart('0'));

        string extra = Empty;

        // protocol V2.0 extra information
        if (input.DataMessage.Bytes.Length > 17)
        {
            const int version = 1;
            const string paramSetAction = "03";
            int time = (int)(DateTime.UtcNow - DateTime.UnixEpoch).TotalSeconds;

            extra = $"{time:X4}{version:X4}{paramSetAction}";
        }

        SendAcknowledge(input, $"{extra}");
    }

    private static void SendAcknowledge(MessageInput input, string extra = "")
    {
        const string mark = "6767";
        string pid = input.DataMessage.Hex[2];
        string sequence = input.DataMessage.Hex[5..7].StringJoin();
        int size = $"{sequence}{extra}".Length / 2;

        string reply = $"{mark}{pid}{size:X4}{sequence}{extra}";

        input.NetworkStream.Write(HexUtil.ConvertHexStringToByteArray(reply));
    }
}