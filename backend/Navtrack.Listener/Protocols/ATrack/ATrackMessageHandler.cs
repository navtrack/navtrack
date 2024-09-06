using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Navtrack.DataAccess.Model.Devices.Messages;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Helpers.New;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.ATrack;

[Service(typeof(ICustomMessageHandler<ATrackProtocol>))]
public class ATrackMessageHandler : BaseMessageHandler<ATrackProtocol>
{
    public override IEnumerable<DeviceMessageDocument>? ParseRange(MessageInput input)
    {
        IEnumerable<DeviceMessageDocument> positions =
            ParseRange(input, HandleKeepAlive, HandleTextMessage, HandleBinaryMessage);

        return positions;
    }

    private static IEnumerable<DeviceMessageDocument> HandleKeepAlive(MessageInput input)
    {
        if (input.DataMessage.Bytes[0] == 0xFE && input.DataMessage.Bytes[1] == 0x02)
        {
            input.NetworkStream.Write(input.DataMessage.Bytes.Skip(2).ToArray());
        }

        return null;
    }

    private static DeviceMessageDocument[] HandleBinaryMessage(MessageInput input)
    {
        input.DataMessage.ByteReader.Skip(2); // prefix
        input.DataMessage.ByteReader.Skip(2); // checksum
        input.DataMessage.ByteReader.Skip(2); // length

        int index = input.DataMessage.ByteReader.GetLe<short>(false);
        byte[] indexBytes = input.DataMessage.ByteReader.Get(2);
        long id = input.DataMessage.ByteReader.GetLe<long>(false);
        byte[] idBytes = input.DataMessage.ByteReader.Get(8);

        input.ConnectionContext.SetDevice($"{id}");

        List<DeviceMessageDocument> positions = [];

        while (input.DataMessage.ByteReader.BytesLeft > 40)
        {
            DeviceMessageDocument position = new()
            {
                // Device = input.ConnectionContext.Device,
                Position = new PositionElement()
            };

            DateTime gpsDate = GetDateTime(input.DataMessage.ByteReader, input);
            DateTime rtcDate = GetDateTime(input.DataMessage.ByteReader, input);
            DateTime sendDate = GetDateTime(input.DataMessage.ByteReader, input);
            position.Position.Date = gpsDate;
            position.Position.Longitude = input.DataMessage.ByteReader.GetLe<int>() * 0.000001f;
            position.Position.Latitude = input.DataMessage.ByteReader.GetLe<int>() * 0.000001f;
            position.Position.Heading = input.DataMessage.ByteReader.GetLe<short>();
            int reportId = input.DataMessage.ByteReader.GetOne();
            position.Device ??= new DeviceElement();
            position.Device.Odometer = (uint?)(input.DataMessage.ByteReader.GetLe<int>() * 100);
            position.Position.HDOP = input.DataMessage.ByteReader.GetLe<short>() * 0.1f;
            byte inputStatus = input.DataMessage.ByteReader.GetOne();
            position.Position.Speed = input.DataMessage.ByteReader.GetLe<short>();
            byte outputStatus = input.DataMessage.ByteReader.GetOne();
            short averageAnalogInput = input.DataMessage.ByteReader.GetLe<short>();
            byte[] driverId = input.DataMessage.ByteReader.GetUntil(0x0);
            short firstTempSensor = input.DataMessage.ByteReader.GetLe<short>();
            short secondTempSensor = input.DataMessage.ByteReader.GetLe<short>();

            byte[] textMessage = input.DataMessage.ByteReader.GetUntil(0x0);

            if (input.DataMessage.ByteReader.BytesLeft > 0)
            {
                byte[] form = input.DataMessage.ByteReader.GetUntil(0x0).Skip(3).ToArray();
                string formString = StringUtil.ConvertByteArrayToString(form);

                ReadCustomData(input, formString, position);
            }

            positions.Add(position);
        }

        SendResponse(input, idBytes, indexBytes);

        return positions.ToArray();
    }

    private static void SendResponse(MessageInput input, in byte[] id, in byte[] index)
    {
        List<byte> response = [0xFE, 0x02];
        response.AddRange(id);
        response.AddRange(index);

        input.NetworkStream.Write(response.ToArray());
    }

    private static void ReadCustomData(MessageInput input, string form, DeviceMessageDocument position)
    {
        string[] fieldIds = form.Split('%');

        foreach (string fieldId in fieldIds)
        {
            switch (fieldId)
            {
                case "SA":
                    input.DataMessage.ByteReader.GetOne();
                    break;
                case "MV":
                    input.DataMessage.ByteReader.Get<short>();
                    break;
                case "BV":
                    input.DataMessage.ByteReader.Get<short>();
                    break;
                case "GQ":
                    input.DataMessage.ByteReader.GetOne();
                    break;
                case "CE":
                    input.DataMessage.ByteReader.Get<int>();
                    break;
                case "LC":
                    input.DataMessage.ByteReader.Get<short>();
                    break;
                case "CN":
                    input.DataMessage.ByteReader.Get<int>();
                    break;
                case "RL":
                    input.DataMessage.ByteReader.GetOne();
                    break;
                case "PC":
                    input.DataMessage.ByteReader.Get<int>();
                    break;
                case "AT":
                    position.Position.Altitude = input.DataMessage.ByteReader.GetLe<int>();
                    break;
                case "RP":
                    input.DataMessage.ByteReader.Get<short>();
                    break;
                case "GS":
                    input.DataMessage.ByteReader.GetOne();
                    break;
                case "DT":
                    input.DataMessage.ByteReader.GetOne();
                    break;
                case "CI":
                    input.DataMessage.ByteReader.GetUntil(0x0);
                    break;
                case "AI1":
                    input.DataMessage.ByteReader.Get<short>();
                    break;
                case "AI2":
                    input.DataMessage.ByteReader.Get<short>();
                    break;
                case "AI3":
                    input.DataMessage.ByteReader.Get<short>();
                    break;
                case "AV1":
                    input.DataMessage.ByteReader.Get<short>();
                    break;
                case "AV2":
                    input.DataMessage.ByteReader.Get<short>();
                    break;
                case "AV3":
                    input.DataMessage.ByteReader.Get<short>();
                    break;
                case "NC":
                    input.DataMessage.ByteReader.GetUntil(0x0);
                    break;
                case "SM":
                    input.DataMessage.ByteReader.Get<short>();
                    break;
                case "GL":
                    input.DataMessage.ByteReader.GetUntil(0x0);
                    break;
                case "MA":
                    input.DataMessage.ByteReader.GetUntil(0x0);
                    break;
                case "PK":
                    input.DataMessage.ByteReader.GetOne();
                    break;
                case "VM":
                    input.DataMessage.ByteReader.GetOne();
                    break;
                case "GV":
                    input.DataMessage.ByteReader.Skip(6);
                    break;
                case "CG":
                    input.DataMessage.ByteReader.GetOne();
                    break;
                case "CS":
                    input.DataMessage.ByteReader.GetOne();
                    break;
                case "TO":
                    input.DataMessage.ByteReader.GetOne();
                    break;
                case "SE1":
                    input.DataMessage.ByteReader.GetOne();
                    break;
                case "SE2":
                    input.DataMessage.ByteReader.GetOne();
                    break;
                case "SE3":
                    input.DataMessage.ByteReader.GetOne();
                    break;
                case "SE4":
                    input.DataMessage.ByteReader.GetOne();
                    break;
                case "SE5":
                    input.DataMessage.ByteReader.GetOne();
                    break;
                case "SE6":
                    input.DataMessage.ByteReader.GetOne();
                    break;
                case "SE7":
                    input.DataMessage.ByteReader.GetOne();
                    break;
                case "SE8":
                    input.DataMessage.ByteReader.GetOne();
                    break;
                case "GP":
                    input.DataMessage.ByteReader.GetOne();
                    break;
                case "TD":
                    input.DataMessage.ByteReader.Get(6);
                    break;
                case "TP1":
                    input.DataMessage.ByteReader.Get<short>();
                    break;
                case "TP2":
                    input.DataMessage.ByteReader.Get<short>();
                    break;
                case "TP3":
                    input.DataMessage.ByteReader.Get<short>();
                    break;
                case "TP4":
                    input.DataMessage.ByteReader.Get<short>();
                    break;
                case "VN":
                    input.DataMessage.ByteReader.GetUntil(0x0);
                    break;
                case "MF":
                    input.DataMessage.ByteReader.Get<short>();
                    break;
                case "EL":
                    input.DataMessage.ByteReader.GetOne();
                    break;
                case "TR":
                    input.DataMessage.ByteReader.GetOne();
                    break;
                case "ET":
                    input.DataMessage.ByteReader.Get<short>();
                    break;
                case "FL":
                    input.DataMessage.ByteReader.GetOne();
                    break;
                case "ML":
                    input.DataMessage.ByteReader.GetOne();
                    break;
                case "FC":
                    input.DataMessage.ByteReader.Get<int>();
                    break;
                case "PD":
                    input.DataMessage.ByteReader.GetOne();
                    break;
                case "CD":
                    input.DataMessage.ByteReader.GetUntil(0x0);
                    break;
                case "CM":
                    input.DataMessage.ByteReader.Get<long>();
                    break;
                case "GN":
                    input.DataMessage.ByteReader.Skip(60);
                    break;
                case "ME":
                    input.DataMessage.ByteReader.Get<long>();
                    break;
                case "IA":
                    input.DataMessage.ByteReader.GetOne();
                    break;
                case "MP":
                    input.DataMessage.ByteReader.GetOne();
                    break;
            }
        }
    }

    private static DateTime GetDateTime(ByteReader byteReader, MessageInput input)
    {
        int year = byteReader.GetLe<short>(false);

        if (year > 2000 && year <= DateTime.UtcNow.AddYears(1).Year)
        {
            DateTime date2 = new(byteReader.GetLe<short>(), byteReader.GetOne(), byteReader.GetOne(),
                byteReader.GetOne(), byteReader.GetOne(), byteReader.GetOne());

            return date2;
        }

        int seconds = byteReader.GetLe<int>();
        DateTime date = DateTime.UnixEpoch.AddSeconds(seconds);

        return date;
    }

    private static DeviceMessageDocument[] HandleTextMessage(MessageInput input)
    {
        StringReader stringReader = new(input.DataMessage.String);

        List<DeviceMessageDocument> positions = [];
        string line;

        while (!string.IsNullOrEmpty(line = stringReader.ReadLine()))
        {
            DeviceMessageDocument position = GetLocation(input, line);

            if (position != null)
            {
                positions.Add(position);
            }
        }

        return positions.Any() ? positions.ToArray() : null;
    }

    private static DeviceMessageDocument GetLocation(MessageInput input, string line)
    {
        Match locationMatch =
            new Regex(
                    "((\\d+)," + // length
                    "(\\d+)," + // seq id
                    "(\\d{15}),)?" + // imei
                    "(\\d{10,14})," + // gps date time
                    "(\\d+)," + // rtc date time
                    "(\\d+)," + // position sending date time
                    "(-?\\d+)," + // longitude
                    "(-?\\d+)," + // latitude
                    "(\\d+)," + // heading
                    "(\\d+)," + // report id
                    "(.*?)," + // odometer
                    "(\\d+)," + // hdop
                    "(\\d+)," + // input status
                    "(\\d+)," + // gps/vss vehicle speed
                    "(\\d+)," + // output status
                    "(\\d+)," + // analog input value
                    "(.*?)," + // driver id
                    "(\\d+)," + // first temperature sensor value
                    "(\\d+)," + // second temperature sensor value
                    "((.*?)," + // text message
                    "(.*?)," + // custom format
                    "(.*))?") // custom data
                .Match(line);

        if (locationMatch.Success)
        {
            input.ConnectionContext.SetDevice(locationMatch.Groups[4].Value);

            DeviceMessageDocument position = new()
            {
                // Device = input.ConnectionContext.Device,
                Position = new PositionElement
                {
                    Date = GetDateTime(locationMatch.Groups[5].Value),
                    Longitude = locationMatch.Groups[8].Get<int>() * 0.000001,
                    Latitude = locationMatch.Groups[9].Get<int>() * 0.000001,
                    Heading = locationMatch.Groups[10].Get<float?>(),
                    HDOP = locationMatch.Groups[13].Get<float?>() * 0.1f,
                    Speed = locationMatch.Groups[15].Get<float>()
                },
                Device = new DeviceElement
                {
                    Odometer = (uint?)(locationMatch.Groups[12].Get<double?>() * 100)
                }
            };

            return position;
        }

        return null;
    }

    private static DateTime GetDateTime(string value)
    {
        return value.Length == 14
            ? NewDateTimeUtil.Convert(DateFormat.YYYYMMDDHHMMSS, value)
            : DateTime.UnixEpoch.AddSeconds(Convert.ToDouble(value));
    }
}