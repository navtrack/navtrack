using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using Navtrack.Library.DI;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Helpers.New;
using Navtrack.Listener.Helpers.New2;
using Navtrack.Listener.Models;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.Navtelecom;

[Service(typeof(ICustomMessageHandler<NavtelecomProtocol>))]
public class NavtelecomMessageHandler : BaseMessageHandler<NavtelecomProtocol>
{
    private static readonly byte[] FlexFieldSize =
    {
        4, 2, 4, 1, 1, 1, 1, 1, 4, 4, 4, 4, 4, 2, 4, 4, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 4, 4, 2, 2,
        4, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 2, 4, 2, 1, 4, 2, 2, 2, 2, 2, 1, 1, 1, 2, 4, 2, 1, 8, 2, 1,
        16, 4, 2, 4, 37, 1, 1, 1, 1, 1, 1, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 6, 12, 24, 48, 1, 1, 2, 1, 4, 4, 1, 4, 2,
        6, 2, 6, 2, 2, 2, 2, 2, 2, 2, 2, 1, 2, 2, 2, 1, 1, 1, 1, 1, 4, 4, 4, 4, 4, 4, 2, 2, 2, 2, 2, 2, 1, 1, 2, 3,
        2, 1, 1, 3, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 2, 4, 4, 4, 2, 4, 2, 2,
        4, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 4, 4, 4, 4, 6, 3, 1, 2, 2, 1, 4, 5, 4, 4, 1, 1, 1, 1, 1, 1, 1, 1,
        1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4,
        4, 4, 8, 8, 8
    };

    private const string FlexArrayKey = "FLEX_ARRAY";

    public override IEnumerable<Location> ParseRange(MessageInput input)
    {
        IEnumerable<Location> location = ParseRange(input,
            Handle_S_Login,
            Handle_FLEX,
            Handle_V1_A_TelemetryArray,
            Handle_V1_T_Telemetry,
            Handle_V1_C_CurrentState,
            Handle_V2_E_TelemetryArray,
            Handle_V2_X_telemetry);

        return location;
    }

    private static IEnumerable<Location> Handle_S_Login(MessageInput input)
    {
        if (input.Client.Device == null)
        {
            Match imeiMatch = new Regex("\\*>S:(\\d{15})").Match(input.DataMessage.String);

            if (imeiMatch.Success)
            {
                input.Client.SetDevice(imeiMatch.Groups[1].Value);

                string hexReply = "*<S".ToHex();
                SendResponseWithTitle(input, hexReply);
            }
        }

        return null;
    }

    private IEnumerable<Location> Handle_FLEX(MessageInput input)
    {
        int? flexStartIndex =
            input.DataMessage.Bytes.GetStartIndex(new byte[] { 0x2A, 0x3E, 0x46, 0x4C, 0x45, 0x58 });

        if (flexStartIndex.HasValue)
        {
            input.DataMessage.ByteReader.Skip(flexStartIndex.Value + 6);
            byte protocol = input.DataMessage.ByteReader.GetOne();
            byte protocolVersion = input.DataMessage.ByteReader.GetOne();
            byte structVersion = input.DataMessage.ByteReader.GetOne();
            byte dataSize = input.DataMessage.ByteReader.GetOne();

            int result = (int)Math.Ceiling((double)dataSize / 8);
            byte[] bytes = input.DataMessage.ByteReader.Get(result);

            bool[] flexArray = new bool[dataSize];
            
            int byteIndex = 0;
            int bitIndex = 0;

            for (int i = 0; i < dataSize; i++)
            {
                flexArray[i] = BitUtil.IsTrue(bytes[byteIndex], 7 - bitIndex++);

                if (bitIndex > 7)
                {
                    bitIndex = 0;
                    byteIndex++;
                }
            }
            
            string hexReply =
                $"{"*<FLEX".ToHex()}{protocol:X2}{protocolVersion:X2}{structVersion:X2}";
            SendResponseWithTitle(input, hexReply);
            input.Client.SetClientCache(FlexArrayKey, flexArray);
        }

        return null;
    }

    private List<Location> Handle_V1_A_TelemetryArray(MessageInput input)
    {
        return input.DataMessage.String.StartsWith("~A")
            ? HandleArrayPackage(input, GetFlex10Location)
            : null;
    }

    private IEnumerable<Location> Handle_V1_T_Telemetry(MessageInput input)
    {
        return input.DataMessage.String.StartsWith("~T")
            ? HandleSinglePackage(input, GetFlex10Location)
            : null;
    }

    private IEnumerable<Location> Handle_V2_E_TelemetryArray(MessageInput input)
    {
        return input.DataMessage.String.StartsWith("~E")
            ? HandleArrayPackage(input, GetFlex20ExtensionLocation)
            : null;
    }

    private IEnumerable<Location> Handle_V2_X_telemetry(MessageInput input)
    {
        return input.DataMessage.String.StartsWith("~X")
            ? HandleSinglePackage(input, GetFlex20ExtensionLocation)
            : null;
    }

    private static List<Location> HandleArrayPackage(MessageInput input,
        Func<MessageInput, ByteReader, Location> locationMapper)
    {
        input.DataMessage.ByteReader.Skip(2);
        byte size = input.DataMessage.ByteReader.GetOne();

        List<Location> locations = new();

        for (int i = 0; i < size; i++)
        {
            Location location = locationMapper(input, input.DataMessage.ByteReader);

            locations.Add(location);
        }

        string response = $"{input.DataMessage.Hex[..2].StringJoin()}" + // header
                          $"{size:X2}";
        SendResponse(response, input.NetworkStream);

        return locations;
    }

    private IEnumerable<Location> HandleSinglePackage(MessageInput input,
        Func<MessageInput, ByteReader, Location> locationMapper)
    {
        input.DataMessage.ByteReader.Skip(6); // header + event index
        Location location = locationMapper(input, input.DataMessage.ByteReader);

        string response = $"{input.DataMessage.Hex[..2].StringJoin()}" + // header
                          $"{input.DataMessage.Hex[2..6].StringJoin()}"; // event Index
        SendResponse(response, input.NetworkStream);

        return new[] { location };
    }

    private IEnumerable<Location> Handle_V1_C_CurrentState(MessageInput input)
    {
        if (input.DataMessage.String.StartsWith("~C"))
        {
            input.DataMessage.ByteReader.Skip(2); // header
            Location location = GetFlex10Location(input, input.DataMessage.ByteReader);

            string response = $"{input.DataMessage.Hex[..2].StringJoin()}";
            SendResponse(response, input.NetworkStream);

            return new[] { location };
        }

        return null;
    }

    private static void SendResponse(string response, INetworkStreamWrapper networkStream)
    {
        byte checksum = ChecksumUtil.Crc8(HexUtil.ConvertHexStringToByteArray(response));
        string replyWithChecksum = $"{response}{checksum:X2}";
        networkStream.Write(HexUtil.ConvertHexStringToByteArray(replyWithChecksum));
    }

    private static void SendResponseWithTitle(MessageInput input, string response)
    {
        byte[] data = HexUtil.ConvertHexStringToByteArray(response);

        string preamble = input.DataMessage.Hex[..4].StringJoin();
        const string receiverId = "00000000";
        string senderId = input.DataMessage.Hex[4..8].StringJoin();
        string dataCount = HexUtil.ConvertByteArrayToHexStringArray(BitConverter.GetBytes((short)data.Length))
            .StringJoin();
        string dataChecksum = ChecksumUtil.Xor(data);
        string reply = $"{preamble}{receiverId}{senderId}{dataCount}{dataChecksum}";
        string checksum = ChecksumUtil.Xor(HexUtil.ConvertHexStringToByteArray(reply));
        string replyWithChecksum = $"{reply}{checksum}{response}";

        input.NetworkStream.Write(HexUtil.ConvertHexStringToByteArray(replyWithChecksum));
    }

    private Location GetFlex10Location(MessageInput input, ByteReader reader)
    {
        Location location = new()
        {
            Device = input.Client.Device
        };

        bool[] flexArray = input.Client.GetClientCache<bool[]>(FlexArrayKey);

        for (int i = 1; i <= flexArray.Length; i++)
        {
            if (flexArray[i - 1])
            {
                switch (i)
                {
                    case 3:
                        location.DateTime = DateTime.UnixEpoch.AddSeconds(reader.Get<int>());
                        break;
                    case 10:
                        location.Latitude = reader.Get<int>() / 600000.0;
                        break;
                    case 11:
                        location.Longitude = reader.Get<int>() / 600000.0;
                        break;
                    case 12:
                        location.Altitude = reader.Get<int>() / 10;
                        break;
                    case 13:
                        location.Speed = reader.Get<float>();
                        break;
                    case 14:
                        location.Heading = reader.Get<short>();
                        break;
                    case 16:
                        location.Odometer = reader.Get<float>() * 1000;
                        break;
                    default:
                        reader.Skip(FlexFieldSize[i - 1]);
                        break;
                }
            }
        }

        return location;
    }

    [SuppressMessage("ReSharper", "UnusedVariable")]
    private static Location GetFlex20ExtensionLocation(MessageInput input, ByteReader reader)
    {
        Location location = new()
        {
            Device = input.Client.Device
        };

        short packageLength = reader.Get<short>();
        Version dataStructureVersion = (Version)reader.GetOne();
        byte staticDataLength = reader.GetOne();
        int packageNumber = reader.Get<int>();
        short eventCode = reader.Get<short>();
        int eventTime = reader.Get<int>();
        location.DateTime = DateTime.UnixEpoch.AddSeconds(eventTime);
        byte navigationSensorState = reader.GetOne();
        int gpsTime = reader.Get<int>();
        location.Latitude = reader.Get<int>() / 600000.0;
        location.Longitude = reader.Get<int>() / 600000.0;
        location.Altitude = reader.Get<int>();
        location.Speed = reader.Get<float>();
        location.Heading = reader.Get<short>();
        location.Odometer = reader.Get<float>() * 1000;

        int dynamicPartLength = packageLength - staticDataLength - 2;
        reader.Skip(dynamicPartLength);

        return location;
    }
}