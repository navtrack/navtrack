using System;
using System.Collections.Generic;
using System.Globalization;
using Navtrack.Library.DI;
using Navtrack.Library.Extensions;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Models;
using Navtrack.Listener.Server;
using static System.String;

namespace Navtrack.Listener.Protocols.Concox;

[Service(typeof(ICustomMessageHandler<ConcoxProtocol>))]
public class ConcoxMessageHandler : BaseMessageHandler<ConcoxProtocol>
{
    private bool ExtendedPacketLength { get; set; }
    private ProtocolNumber ProtocolNumber { get; set; }

    public override Location Parse(MessageInput input)
    {
        ExtendedPacketLength = input.DataMessage.Bytes[0] == 0x79 && input.DataMessage.Bytes[1] == 0x79;
        ProtocolNumber = (ProtocolNumber)input.DataMessage.Bytes[GetIndex(3)];

        Dictionary<ProtocolNumber, Func<MessageInput, Location>> methods =
            new()
            {
                { ProtocolNumber.LoginInformation, LoginInformationHandler },
                { ProtocolNumber.Heartbeat, HeartbeatHandler },
                { ProtocolNumber.PositioningData, PositioningDataHandler },
                { ProtocolNumber.PositioningDataNew, PositioningDataHandler }
            };

        return methods.ContainsKey(ProtocolNumber) ? methods[ProtocolNumber](input) : null;
    }

    private Location PositioningDataHandler(MessageInput input)
    {
        string courseAndStatus =
            Convert.ToString(input.DataMessage.Bytes[GetIndex(20)], 2).PadLeft(8, '0') +
            Convert.ToString(input.DataMessage.Bytes[GetIndex(21)], 2).PadLeft(8, '0');

        CardinalPoint longitudeCardinalPoint = courseAndStatus[4] == '0' ? CardinalPoint.East : CardinalPoint.West;
        CardinalPoint latitudeCardinalPoint = courseAndStatus[5] == '1' ? CardinalPoint.North : CardinalPoint.South;

        Location location = new()
        {
            Device = input.Client.Device,
            DateTime = DateTimeUtil.NewFromHex(input.DataMessage.Hex[GetIndex(4)], input.DataMessage.Hex[GetIndex(5)],
                input.DataMessage.Hex[GetIndex(6)], input.DataMessage.Hex[GetIndex(7)],
                input.DataMessage.Hex[GetIndex(8)],
                input.DataMessage.Hex[GetIndex(9)]),
            Satellites = short.Parse($"{input.DataMessage.Hex[GetIndex(10)][1]}", NumberStyles.HexNumber),
            Latitude = GetCoordinate(input.DataMessage.Hex.SubArray(GetIndex(11), GetIndex(15)),
                latitudeCardinalPoint),
            Longitude = GetCoordinate(input.DataMessage.Hex.SubArray(GetIndex(15), GetIndex(19)),
                longitudeCardinalPoint),
            Speed = input.DataMessage.Bytes[GetIndex(19)],
            PositionStatus = courseAndStatus[3] == '1',
            Heading = Convert.ToInt32(courseAndStatus[6..], 2),
            MobileCountryCode = int.Parse(
                Join(Empty, input.DataMessage.Hex.SubArray(GetIndex(22), GetIndex(24))), NumberStyles.HexNumber),
            MobileNetworkCode = input.DataMessage.Bytes[GetIndex(24)],
            LocationAreaCode = int.Parse(
                Join(Empty, input.DataMessage.Hex.SubArray(GetIndex(25), GetIndex(27))), NumberStyles.HexNumber),
            CellId = int.Parse(Join(Empty, input.DataMessage.Hex.SubArray(GetIndex(27), GetIndex(30))),
                NumberStyles.HexNumber)
        };

        return location;
    }

    private Location LoginInformationHandler(MessageInput input)
    {
        try
        {
            string imei = GetImei(input.DataMessage.Hex);
            string[] serialNumber = { "00", "01" };

            if (StringUtil.IsDigitsOnly(imei))
            {
                input.Client.SetDevice(imei);

                ConcoxOutputMessage output = new(ProtocolNumber, serialNumber);

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

    private Location HeartbeatHandler(MessageInput input)
    {
        try
        {
            string[] serialNumber = input.DataMessage.Hex[^6..^4];

            ConcoxOutputMessage output = new(ProtocolNumber, serialNumber);
            input.NetworkStream.Write(HexUtil.ConvertHexStringToByteArray(output.PacketFull));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        return null;
    }

    private string GetImei(string[] input)
    {
        string imei = Join(Empty, input.SubArray(GetIndex(4), GetIndex(12)));

        if (imei.StartsWith("0"))
        {
            imei = imei[1..];
        }

        return imei;
    }

    private static double GetCoordinate(string[] strings, CardinalPoint cardinalPoint)
    {
        double d = int.Parse(Join(Empty, strings), NumberStyles.HexNumber);
        int degrees = (int)(d / 30000 / 60);
        double minutes = d / 30000 - degrees * 60;

        return GpsUtil.ConvertDdmToDecimal(degrees, minutes, cardinalPoint);
    }

    private int GetIndex(int index)
    {
        return ExtendedPacketLength ? index + 1 : index;
    }
}