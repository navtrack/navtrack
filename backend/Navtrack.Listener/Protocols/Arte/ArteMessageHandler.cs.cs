using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Helpers.New;
using Navtrack.Listener.Models;
using Navtrack.Listener.Protocols.Arusnavi;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.Arte;

[Service(typeof(ICustomMessageHandler<ArteProtocol>))]
public class ArteMessageHandler : BaseMessageHandler<ArteProtocol>
{
    public override IEnumerable<Position>? ParseRange(MessageInput input)
    {
        IEnumerable<Position> location =
            ParseRange(input, ParseTextMessage, ParseBinaryLogin, ParseLocationMessage);

        return location;
    }

    private IEnumerable<Position> ParseLocationMessage(MessageInput input)
    {
        List<Position> positions = new List<Position>();

        byte parcelNumber = input.DataMessage.ByteReader.Skip(1).GetOne();

        while (input.DataMessage.ByteReader.BytesLeft > 1)
        {
            byte packetType = input.DataMessage.ByteReader.GetOne();
            int length = input.DataMessage.ByteReader.Get<short>();
            DateTime dateTime = DateTime.UnixEpoch.AddSeconds(input.DataMessage.ByteReader.Get<int>());

            positions.Add(GetLocation(input, dateTime, length));

            input.DataMessage.ByteReader.Skip(1); // checksum
        }

        SendResponse(input, (byte)HeaderVersion.V1, parcelNumber);

        return positions;
    }

    private Position GetLocation(MessageInput input, in DateTime dateTime, int length)
    {
        Position position = new()
        {
            Device = input.ConnectionContext.Device ?? throw new InvalidOperationException("Device is null"),
            Date = dateTime
        };

        int bytesRead = 0;
        while (bytesRead < length)
        {
            short tag = input.DataMessage.ByteReader.GetOne();

            switch (tag)
            {
                case 3:
                    position.Latitude = input.DataMessage.ByteReader.Get<float>();
                    break;

                case 4:
                    position.Longitude = input.DataMessage.ByteReader.Get<float>();
                    break;

                case 5:
                    position.Speed = SpeedUtil.KnotsToKph(input.DataMessage.ByteReader.GetOne());
                    byte satellites = input.DataMessage.ByteReader.GetOne();
                    position.Satellites = (short)((satellites & 0x0F) + ((satellites >> 4) & 0x0F));
                    position.Altitude = input.DataMessage.ByteReader.GetOne() * 10;
                    position.Heading = input.DataMessage.ByteReader.GetOne() * 2;
                    break;

                default:
                    input.DataMessage.ByteReader.Skip(4);
                    break;
            }

            bytesRead += 5; // tag id (1) + tag data (4)
        }

        return position;
    }

    private static IEnumerable<Position> ParseBinaryLogin(MessageInput input)
    {
        if (input.DataMessage.ByteReader.GetOne() == 0xFF)
        {
            byte headerVersion = input.DataMessage.ByteReader.GetOne();
            long imei = input.DataMessage.ByteReader.Get<long>();

            input.ConnectionContext.SetDevice($"{imei}");

            SendResponse(input, headerVersion, 0);
        }

        return Array.Empty<Position>();
    }

    private static void SendResponse(MessageInput input, byte version, int index)
    {
        List<byte> response = new List<byte> { 0x7B };

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

    private static IEnumerable<Position> ParseTextMessage(MessageInput input)
    {
        Match locationMatch =
            new Regex(
                    "\\[M" +
                    "(?<IMEI>\\d{15})" + // IMEI
                    "(?<Date>\\d{6})" + // Date (DDMMYY)
                    "(?<Time>\\d{6})" + // Time (HHMMSS)
                    "(?<Lat>[NS]\\d+\\.\\d+)" + // Latitude
                    "(?<Lon>[EW]\\d+\\.\\d+)" + // Longitude
                    "(?<Fix>[01])" + // GPS fix (0: invalid, 1: valid)
                    "(?<Sat>\\w{1})" + // Satellites (HEX)
                    "(?<Speed>\\w{2})" + // Speed (HEX)
                    "(?<Heading>\\d{3})" + // Heading (degrees)
                    "(?<AIN>\\w{6})" + // Analog inputs (HEX)
                    "(?<Status>\\w{2})" + // Status (HEX)
                    "(?<MaxSpeed>\\w{2})" + // Max speed (HEX)
                    "(?<Distance>\\w{6})" + // Distance (HEX)
                    "(?<Blank>\\w{4})" + // Blank (usually FFFF)
                    "(?<Alarms>:[\\w]+)?" + // Active alarms (optional)
                    "!(?<DriverID>[\\w]+)?" + // Driver ID (optional)
                    "\\]").Match(input.DataMessage.String);

        if (locationMatch.Success)
        {
            input.ConnectionContext.SetDevice(locationMatch.Groups["IMEI"].Value);

            Position position = new()
            {
                Device = input.ConnectionContext.Device ?? throw new InvalidOperationException("Device is null"),
                Date = NewDateTimeUtil.Convert(DateFormat.DDMMYYHHMMSS,
                    $"{locationMatch.Groups["Date"].Value}{locationMatch.Groups["Time"].Value}"),
                Latitude = GpsUtil.ConvertDmmLatToDecimal(locationMatch.Groups["Lat"].Value, locationMatch.Groups["Lat"].Value.Substring(0, 1)),
                Longitude = GpsUtil.ConvertDmmLatToDecimal(locationMatch.Groups["Lon"].Value, locationMatch.Groups["Lon"].Value.Substring(0, 1)),
                Speed = 0, // Assuming a new method or conversion needs to be implemented for Speed
                Heading = int.Parse(locationMatch.Groups["Heading"].Value),
                Satellites = (short?)int.Parse(locationMatch.Groups["Sat"].Value, System.Globalization.NumberStyles.HexNumber),
                Altitude = locationMatch.Groups["Alarms"].Success ? int.Parse(locationMatch.Groups["Alarms"].Value) * 10 : 0,
            };

            return new[] { position };
        }

        return Array.Empty<Position>();
    }
}