using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Navtrack.Library.DI;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Helpers.New;
using Navtrack.Listener.Models;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.Arusnavi;

[Service(typeof(ICustomMessageHandler<ArusnaviProtocol>))]
public class ArusnaviMessageHandler : BaseMessageHandler<ArusnaviProtocol>
{
    public override IEnumerable<Location> ParseRange(MessageInput input)
    {
        IEnumerable<Location> location =
            ParseRange(input, ParseTextMessage, ParseBinaryLogin, ParseLocationMessage);

        return location;
    }

    private IEnumerable<Location> ParseLocationMessage(MessageInput input)
    {
        List<Location> locations = new();

        byte parcelNumber = input.DataMessage.ByteReader.Skip(1).GetOne();

        while (input.DataMessage.ByteReader.BytesLeft > 1)
        {
            byte packetType = input.DataMessage.ByteReader.GetOne();
            int length = input.DataMessage.ByteReader.Get<short>();
            DateTime dateTime = DateTime.UnixEpoch.AddSeconds(input.DataMessage.ByteReader.Get<int>());

            locations.Add(GetLocation(input, dateTime, length));

            input.DataMessage.ByteReader.Skip(1); // checksum
        }

        SendResponse(input, (byte)HeaderVersion.V1, parcelNumber);

        return locations;
    }

    private Location GetLocation(MessageInput input, in DateTime dateTime, int length)
    {
        Location location = new()
        {
            Device = input.Client.Device,
            DateTime = dateTime
        };

        int bytesRead = 0;
        while (bytesRead < length)
        {
            short tag = input.DataMessage.ByteReader.GetOne();

            switch (tag)
            {
                case 3:
                    location.Latitude = input.DataMessage.ByteReader.Get<float>();
                    break;

                case 4:
                    location.Latitude = input.DataMessage.ByteReader.Get<float>();
                    break;

                case 5:
                    location.Speed = SpeedUtil.KnotsToKph(input.DataMessage.ByteReader.GetOne());
                    byte satellites = input.DataMessage.ByteReader.GetOne();
                    location.Satellites = (short)(satellites & 0x0F + (satellites >> 4) & 0x0F);
                    location.Altitude = input.DataMessage.ByteReader.GetOne() * 10;
                    location.Heading = input.DataMessage.ByteReader.GetOne() * 2;
                    break;

                default:
                    input.DataMessage.ByteReader.Skip(4);
                    break;
            }

            bytesRead += 5; // tag id (1) + tag data (4)
        }

        return location;
    }

    private static IEnumerable<Location> ParseBinaryLogin(MessageInput input)
    {
        if (input.DataMessage.ByteReader.GetOne() == 0xFF)
        {
            byte headerVersion = input.DataMessage.ByteReader.GetOne();
            long imei = input.DataMessage.ByteReader.Get<long>();

            input.Client.SetDevice($"{imei}");

            SendResponse(input, headerVersion, 0);
        }

        return null;
    }

    private static void SendResponse(MessageInput input, byte version, int index)
    {
        List<byte> response = new() { 0x7B };

        if (version == (int)HeaderVersion.V1)
        {
            response.Add(0x00);
            response.Add((byte)index);
        }
        else if (version == (int)HeaderVersion.V2)
        {
            response.Add(0x04);
            response.Add(0x00);
            byte[] timeBytes = BitConverter.GetBytes((int)DateTimeOffset.UtcNow.ToUnixTimeSeconds());
            response.Add((byte)ChecksumUtil.Modulo256(timeBytes));
            response.AddRange(timeBytes);
        }

        response.Add(0x7D);
        input.NetworkStream.Write(response.ToArray());
    }

    private static IEnumerable<Location> ParseTextMessage(MessageInput input)
    {
        Match locationMatch =
            new Regex(
                    "(\\$AV)," +
                    "(V\\d)," + // type
                    "(\\d+)," + // device id
                    "(\\d+)," + // index
                    "(\\d+)," + // power
                    "(\\d+)," + // battery
                    "(-?\\d+)," +
                    "(0|1)," + // movement
                    "(0|1)," + // ignition
                    "\\d+," + // input
                    "(\\d+,\\d+)," + // input 1
                    "((\\d+,\\d+),)?" + // input 2
                    "(0|1)," + // fix type gps / glonass
                    "(\\d+)," + // satellites
                    "((\\d+\\.\\d+))?.?" + // altitude
                    "((\\d+\\.\\d+).)?" + // 
                    "(\\d{6})," + // time hh mm ss
                    "(\\d+.\\d+)(N|S)," + // latitude
                    "(\\d+.\\d+)(E|W)," + // longitude
                    "(\\d+.\\d+)," + // speed
                    "(\\d+.\\d+)," + // heading
                    "(\\d{6})") // date dd mm yy
                .Match(input.DataMessage.String);

        if (locationMatch.Success)
        {
            input.Client.SetDevice(locationMatch.Groups[3].Value);

            Location location = new()
            {
                Device = input.Client.Device,
                Satellites = locationMatch.Groups[14].Get<short?>(),
                Altitude = locationMatch.Groups[15].Get<float?>(),
                DateTime = NewDateTimeUtil.Convert(DateFormat.DDMMYYHHMMSS,
                    $"{locationMatch.Groups[26].Value}{locationMatch.Groups[19].Value}"),
                Latitude = GpsUtil.ConvertDmmLatToDecimal(locationMatch.Groups[20].Value,
                    locationMatch.Groups[21].Value),
                Longitude = GpsUtil.ConvertDmmLatToDecimal(locationMatch.Groups[22].Value,
                    locationMatch.Groups[23].Value),
                Speed = SpeedUtil.KnotsToKph(locationMatch.Groups[24].Get<float>()),
                Heading = locationMatch.Groups[25].Get<float?>(),
            };

            return new[] { location };
        }

        return null;
    }
}