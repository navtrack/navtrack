using System;
using System.Collections.Generic;
using System.Linq;
using Navtrack.DataAccess.Model.Devices.Messages;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Helpers.New;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.Autofon;

[Service(typeof(ICustomMessageHandler<AutofonProtocol>))]
public class AutofonMessageHandler : BaseMessageHandler<AutofonProtocol>
{
    public override IEnumerable<DeviceMessageDocument>? ParseRange(MessageInput input)
    {
        MessageType type = (MessageType)input.DataMessage.ByteReader.GetOne();

        return type switch
        {
            MessageType.LoginV1 => HandleLoginMessage(input, type),
            MessageType.LoginV2 => HandleLoginMessage(input, type),
            MessageType.LocationV1 => HandleLocationV1Message(input),
            MessageType.HistoryV1 => HandleHistoryV1Message(input),
            MessageType.LocationV2 => HandleLocationV2Message(input),
            _ => null
        };
    }

    private static IEnumerable<DeviceMessageDocument> HandleLoginMessage(MessageInput input, MessageType type)
    {
        if (type == MessageType.LoginV1)
        {
            input.DataMessage.ByteReader.Skip(2);
        }

        input.ConnectionContext.SetDevice(HexUtil.ConvertByteArrayToHexStringArray(input.DataMessage.ByteReader.Get(8))
            .StringJoin().Substring(1));

        SendLoginResponse(input);

        return null;
    }

    private static DeviceMessageDocument[] HandleLocationV1Message(MessageInput input, bool history = false)
    {
        DeviceMessageDocument deviceMessageDocument = new()
        {
            // Device = input.ConnectionContext.Device,
            Position = new PositionElement()
        };

        input.DataMessage.ByteReader.Skip(history ? 18 : 53);

        int valid = input.DataMessage.ByteReader.GetOne();
        deviceMessageDocument.Position.Valid = (valid & 0xc0) != 0;
        deviceMessageDocument.Position.Satellites = (short?)(valid & 0x3f);
        deviceMessageDocument.Position.Date = GetDateTime(input);
        deviceMessageDocument.Position.Latitude = GetCoordinate(input.DataMessage.ByteReader.GetLe<int>());
        deviceMessageDocument.Position.Longitude = GetCoordinate(input.DataMessage.ByteReader.GetLe<int>());
        deviceMessageDocument.Position.Altitude = input.DataMessage.ByteReader.GetLe<short>();
        deviceMessageDocument.Position.Speed = SpeedUtil.KnotsToKph(input.DataMessage.ByteReader.GetOne());
        deviceMessageDocument.Position.Heading = input.DataMessage.ByteReader.GetOne() * 2.0f;
        deviceMessageDocument.Position.HDOP = input.DataMessage.ByteReader.GetLe<short>();

        input.DataMessage.ByteReader.Skip(3);

        return [deviceMessageDocument];
    }

    private static IEnumerable<DeviceMessageDocument> HandleHistoryV1Message(MessageInput input)
    {
        int count = input.DataMessage.ByteReader.GetOne() & 0x0f;
        int totalCount = input.DataMessage.ByteReader.Get<short>();
        List<DeviceMessageDocument> positions = [];

        for (int i = 0; i < count; i++)
        {
            positions.AddRange(HandleLocationV1Message(input, true));
        }

        return positions;
    }

    private static IEnumerable<DeviceMessageDocument> HandleLocationV2Message(MessageInput input)
    {
        DeviceMessageDocument deviceMessageDocument = new()
        {
            // Device = input.ConnectionContext.Device,
            Position = new PositionElement()
        };

        input.DataMessage.ByteReader.Skip(14);

        int valid = input.DataMessage.ByteReader.GetOne();
        deviceMessageDocument.Position.Valid = BitUtil.ShiftRight(valid, 6) != 0;
        deviceMessageDocument.Position.Satellites = (short?)BitUtil.ShiftRight(valid, 6);
        deviceMessageDocument.Position.Date = GetDateTimeV2(input);
        deviceMessageDocument.Position.Latitude = GetCoordinate(input.DataMessage.ByteReader.GetOne(),
            input.DataMessage.ByteReader.GetMediumIntLe());
        deviceMessageDocument.Position.Longitude = GetCoordinate(input.DataMessage.ByteReader.GetOne(),
            input.DataMessage.ByteReader.GetMediumIntLe());
        deviceMessageDocument.Position.Speed = SpeedUtil.KnotsToKph(input.DataMessage.ByteReader.GetOne());
        deviceMessageDocument.Position.Heading = input.DataMessage.ByteReader.GetLe<short>();

        return new[] { deviceMessageDocument };
    }

    private static void SendLoginResponse(MessageInput input)
    {
        List<byte> response = StringUtil.ConvertStringToByteArray("resp_crc=").ToList();
        response.Add(input.DataMessage.Bytes[^1]);

        input.NetworkStream.Write(response.ToArray());
    }

    private static double GetCoordinate(short degrees, int minutes)
    {
        double value = degrees + BitUtil.ShiftRight(minutes, 4) / 600000.0f;

        return BitUtil.IsTrue(minutes, 0) ? value : -value;
    }

    private static double GetCoordinate(int value)
    {
        int degrees = value / 1000000;
        double minutes = value % 1000000 / 10000.0f;
        return degrees + minutes / 60f;
    }

    private static DateTime GetDateTime(MessageInput input)
    {
        int day = input.DataMessage.ByteReader.GetOne();
        int month = input.DataMessage.ByteReader.GetOne();
        int year = input.DataMessage.ByteReader.GetOne();
        int hour = input.DataMessage.ByteReader.GetOne();
        int min = input.DataMessage.ByteReader.GetOne();
        int sec = input.DataMessage.ByteReader.GetOne();

        return new DateTime(2000 + year, month, day, hour, min, sec);
    }

    private static DateTime GetDateTimeV2(MessageInput input)
    {
        int time = input.DataMessage.ByteReader.GetMediumIntLe();
        int date = input.DataMessage.ByteReader.GetMediumIntLe();

        return new DateTime(2000 + date % 100, date / 100 % 100, date / 10000, time / 10000,
            time / 100 % 100, time % 100);
    }
}