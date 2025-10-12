using System;
using System.Collections.Generic;
using Navtrack.Database.Model.Devices;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;
using static System.String;

namespace Navtrack.Listener.Protocols.Eelink;

[Service(typeof(ICustomMessageHandler<EelinkProtocol>))]
public class EelinkMessageHandler : BaseMessageHandler<EelinkProtocol>
{
    public override DeviceMessageEntity? Parse(MessageInput input)
    {
        PackageIdentifier packageIdentifier = (PackageIdentifier)input.DataMessage.Bytes[2];

        Dictionary<PackageIdentifier, Func<DeviceMessageEntity?>> dictionary = new()
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
                    DeviceMessageEntity deviceMessage = GetLocationV20(input);

                    string number =
                        HexUtil.ConvertHexStringArrayToHexString(
                            HexUtil.ConvertByteArrayToHexStringArray(input.DataMessage.ByteReader.Get(21)));

                    SendAcknowledge(input, number);

                    return deviceMessage;
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

    private static DeviceMessageEntity HandleLocationForV20(MessageInput input)
    {
        DeviceMessageEntity deviceMessage = GetLocationV20(input);

        SendAcknowledge(input);

        return deviceMessage;
    }

    private static DeviceMessageEntity HandleMessageForV18(MessageInput input, bool sendAcknowledge = true)
    {
        DeviceMessageEntity deviceMessage = GetLocationV18(input, 7);

        if (sendAcknowledge)
        {
            SendAcknowledge(input);
        }

        return deviceMessage;
    }

    private static DeviceMessageEntity GetLocationV18(MessageInput input, int startIndex)
    {
        DeviceMessageEntity deviceMessage = new();

        input.DataMessage.ByteReader.Skip(startIndex);
        deviceMessage.Date =
            DateTime.UnixEpoch.AddSeconds(input.DataMessage.ByteReader.Get(4).ToSInt4());
        deviceMessage.Latitude = input.DataMessage.ByteReader.Get(4).ToSInt4() / 1800000.0;
        deviceMessage.Longitude = input.DataMessage.ByteReader.Get(4).ToSInt4() / 1800000.0;
        deviceMessage.Speed = input.DataMessage.ByteReader.Get(2).ToSShort2();
        deviceMessage.Heading = input.DataMessage.ByteReader.Get(2).ToSShort2();
        deviceMessage.GSMMobileCountryCode = input.DataMessage.ByteReader.Get(2).ToSShort2().ToString();
        deviceMessage.GSMMobileNetworkCode = input.DataMessage.ByteReader.Get(2).ToSShort2().ToString();
        deviceMessage.GSMLocationAreaCode = input.DataMessage.ByteReader.Get(2).ToSShort2().ToString();
        deviceMessage.GSMCellId = GetCellId(input.DataMessage.ByteReader);

        string status = Convert.ToString(input.DataMessage.ByteReader.GetOne(), 2).PadLeft(8, '0');
        deviceMessage.Valid = status[^1] == '1';

        return deviceMessage;
    }

    private static int GetCellId(ByteReader dataMessageByteReader)
    {
        byte[] array =
            [0x00, dataMessageByteReader.GetOne(), dataMessageByteReader.GetOne(), dataMessageByteReader.GetOne()];

        return array.ToSInt4();
    }

    private static DeviceMessageEntity GetLocationV20(MessageInput input)
    {
        const int locationStartIndex = 7;

        DeviceMessageEntity deviceMessage = new();

        input.DataMessage.ByteReader.Skip(locationStartIndex);
        deviceMessage.Date =
            DateTime.UnixEpoch.AddSeconds(input.DataMessage.ByteReader.Get(4).ToSInt4());
        string mask = Convert.ToString(input.DataMessage.ByteReader.GetOne(), 2).PadLeft(8, '0');

        if (mask[^1] == '1')
        {
            deviceMessage.Latitude = input.DataMessage.ByteReader.Get(4).ToSInt4() / 1800000.0;
            deviceMessage.Longitude = input.DataMessage.ByteReader.Get(4).ToSInt4() / 1800000.0;
            deviceMessage.Altitude = input.DataMessage.ByteReader.Get(2).ToSShort2();
            deviceMessage.Speed = input.DataMessage.ByteReader.Get(2).ToSShort2();
            deviceMessage.Heading = input.DataMessage.ByteReader.Get(2).ToSShort2();
            deviceMessage.Satellites = Convert.ToInt16(input.DataMessage.ByteReader.GetOne());
        }

        if (mask[^2] == '1')
        {
            deviceMessage.GSMMobileCountryCode = input.DataMessage.ByteReader.Get(2).ToSShort2().ToString();
            deviceMessage.GSMMobileNetworkCode = input.DataMessage.ByteReader.Get(2).ToSShort2().ToString();
            deviceMessage.GSMLocationAreaCode = input.DataMessage.ByteReader.Get(2).ToSShort2().ToString();
            deviceMessage.GSMCellId = input.DataMessage.ByteReader.Get(4).ToSInt4();
            deviceMessage.GSMSignalStrength = input.DataMessage.ByteReader.GetOne();
        }

        return deviceMessage;
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