using System;
using System.Collections.Generic;
using System.Globalization;
using Navtrack.Library.DI;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Models;
using Navtrack.Listener.Server;
using static System.String;

namespace Navtrack.Listener.Protocols.Concox;

[Service(typeof(ICustomMessageHandler<ConcoxProtocol>))]
public class ConcoxMessageHandler : BaseMessageHandler<ConcoxProtocol>
{
    public override Location Parse(MessageInput input)
    {
        ProtocolNumber protocolNumber = (ProtocolNumber) input.DataMessage.Bytes[3];

        Dictionary<ProtocolNumber, Func<MessageInput, Location>> methods =
            new()
            {
                {ProtocolNumber.LoginInformation, LoginInformationHandler},
                {ProtocolNumber.Heartbeat, HeartbeatHandler},
                {ProtocolNumber.PositioningData, PositioningDataHandler},
                {ProtocolNumber.PositioningData2, PositioningDataHandler}
            };

        return methods.ContainsKey(protocolNumber) ? methods[protocolNumber](input) : null;
    }

    private static Location PositioningDataHandler(MessageInput input)
    {
        string courseAndStatus = Convert.ToString(input.DataMessage.Bytes[20], 2).PadLeft(8, '0') +
                                 Convert.ToString(input.DataMessage.Bytes[21], 2).PadLeft(8, '0');

        CardinalPoint longitudeCardinalPoint = courseAndStatus[4] == '0' ? CardinalPoint.East : CardinalPoint.West;
        CardinalPoint latitudeCardinalPoint = courseAndStatus[5] == '1' ? CardinalPoint.North : CardinalPoint.South;

        Location location = new()
        {
            Device = input.Client.Device,
            DateTime = DateTimeUtil.NewFromHex(input.DataMessage.Hex[4], input.DataMessage.Hex[5],
                input.DataMessage.Hex[6], input.DataMessage.Hex[7], input.DataMessage.Hex[8],
                input.DataMessage.Hex[9]),
            Satellites = short.Parse($"{input.DataMessage.Hex[10][1]}", NumberStyles.HexNumber),
            Latitude = GetCoordinate(input.DataMessage.Hex[11..15], latitudeCardinalPoint),
            Longitude = GetCoordinate(input.DataMessage.Hex[15..19], longitudeCardinalPoint),
            Speed = input.DataMessage.Bytes[19],
            PositionStatus = courseAndStatus[3] == '1',
            Heading = Convert.ToInt32(courseAndStatus.Substring(6), 2),
            MobileCountryCode = int.Parse(Join(Empty, input.DataMessage.Hex[22..24]), NumberStyles.HexNumber),
            MobileNetworkCode = input.DataMessage.Bytes[24],
            LocationAreaCode = int.Parse(Join(Empty, input.DataMessage.Hex[25..27]), NumberStyles.HexNumber),
            CellId = int.Parse(Join(Empty, input.DataMessage.Hex[27..30]), NumberStyles.HexNumber)
        };

        return location;
    }

    private static decimal GetCoordinate(string[] strings, CardinalPoint cardinalPoint)
    {
        decimal d = int.Parse(Join(Empty, strings), NumberStyles.HexNumber);
        int degrees = (int) (d / 30000 / 60);
        decimal minutes = d / 30000 - degrees * 60;

        return GpsUtil.ConvertDmmToDecimal(degrees, minutes, cardinalPoint);
    }

    private static Location LoginInformationHandler(MessageInput input)
    {
        try
        {
            string imei = GetImei(input.DataMessage.Hex);
            string[] serialNumber = { "00", "01" };
            ProtocolNumber protocolNumber = (ProtocolNumber) input.DataMessage.Bytes[3];

            if (StringUtil.IsDigitsOnly(imei))
            {
                input.Client.SetDevice(imei);

                ConcoxOutputMessage output = new(protocolNumber, serialNumber);

                input.NetworkStream.Write(HexUtil.ConvertHexStringToByteArray(output.PacketFull));
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        return null;
    }

    private static Location HeartbeatHandler(MessageInput input)
    {
        try
        {
            string[] serialNumber = input.DataMessage.Hex[^6..^4];
            ProtocolNumber protocolNumber = (ProtocolNumber) input.DataMessage.Bytes[3];

            ConcoxOutputMessage output = new(protocolNumber, serialNumber);
            input.NetworkStream.Write(HexUtil.ConvertHexStringToByteArray(output.PacketFull));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        return null;
    }

    private static string GetImei(string[] input)
    {
        string imei = Join(Empty, input[4..12]);

        if (imei.StartsWith("0"))
        {
            imei = imei.Substring(1);
        }

        return imei;
    }
}