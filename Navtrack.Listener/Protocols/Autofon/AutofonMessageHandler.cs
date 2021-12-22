using System;
using System.Collections.Generic;
using System.Linq;
using Navtrack.Library.DI;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Helpers.New;
using Navtrack.Listener.Models;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.Autofon;

[Service(typeof(ICustomMessageHandler<AutofonProtocol>))]
public class AutofonMessageHandler : BaseMessageHandler<AutofonProtocol>
{
    public override IEnumerable<Location> ParseRange(MessageInput input)
    {
        MessageType type = (MessageType) input.DataMessage.ByteReader.GetOne();

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

    private static IEnumerable<Location> HandleLoginMessage(MessageInput input, MessageType type)
    {
        if (type == MessageType.LoginV1)
        {
            input.DataMessage.ByteReader.Skip(2);
        }

        input.Client.SetDevice(HexUtil.ConvertByteArrayToHexStringArray(input.DataMessage.ByteReader.Get(8))
            .StringJoin().Substring(1));
            
        SendLoginResponse(input);

        return null;
    }

    private static Location[] HandleLocationV1Message(MessageInput input, bool history = false)
    {
        Location location = new()
        {
            Device = input.Client.Device
        };

        input.DataMessage.ByteReader.Skip(history ? 18 : 53);

        int valid = input.DataMessage.ByteReader.GetOne();
        location.PositionStatus = (valid & 0xc0) != 0;
        location.Satellites = (short?) (valid & 0x3f);
        location.DateTime = GetDateTime(input);
        location.Latitude = GetCoordinate(input.DataMessage.ByteReader.GetLe<int>());
        location.Longitude = GetCoordinate(input.DataMessage.ByteReader.GetLe<int>());
        location.Altitude = input.DataMessage.ByteReader.GetLe<short>();
        location.Speed = SpeedUtil.KnotsToKph(input.DataMessage.ByteReader.GetOne());
        location.Heading = input.DataMessage.ByteReader.GetOne() * 2.0f;
        location.HDOP = input.DataMessage.ByteReader.GetLe<short>();

        input.DataMessage.ByteReader.Skip(3);

        return new[] {location};
    }

    private static IEnumerable<Location> HandleHistoryV1Message(MessageInput input)
    {
        int count = input.DataMessage.ByteReader.GetOne() & 0x0f;
        int totalCount = input.DataMessage.ByteReader.Get<short>();
        List<Location> locations = new();

        for (int i = 0; i < count; i++)
        {
            locations.AddRange(HandleLocationV1Message(input, true));
        }

        return locations;
    }

    private static IEnumerable<Location> HandleLocationV2Message(MessageInput input)
    {
        Location location = new()
        {
            Device = input.Client.Device
        };

        input.DataMessage.ByteReader.Skip(14);

        int valid = input.DataMessage.ByteReader.GetOne();
        location.PositionStatus = BitUtil.ShiftRight(valid, 6) != 0;
        location.Satellites = (short?) BitUtil.ShiftRight(valid, 6);
        location.DateTime = GetDateTimeV2(input);
        location.Latitude = GetCoordinate(input.DataMessage.ByteReader.GetOne(),
            input.DataMessage.ByteReader.GetMediumIntLe());
        location.Longitude = GetCoordinate(input.DataMessage.ByteReader.GetOne(),
            input.DataMessage.ByteReader.GetMediumIntLe());
        location.Speed = SpeedUtil.KnotsToKph(input.DataMessage.ByteReader.GetOne());
        location.Heading = input.DataMessage.ByteReader.GetLe<short>();

        return new[] {location};
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