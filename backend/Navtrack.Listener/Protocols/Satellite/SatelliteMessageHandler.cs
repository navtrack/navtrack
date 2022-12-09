using System;
using System.Collections.Generic;
using Navtrack.Library.DI;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Helpers.Crc;
using Navtrack.Listener.Models;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.Satellite;

[Service(typeof(ICustomMessageHandler<SatelliteProtocol>))]
public class SatelliteMessageHandler : BaseMessageHandler<SatelliteProtocol>
{
    public override IEnumerable<Location> ParseRange(MessageInput input)
    {
        short checksum = input.DataMessage.ByteReader.Get<short>();
        short preamble = input.DataMessage.ByteReader.Get<short>();
        long id = input.DataMessage.ByteReader.Get<int>();
        short length = input.DataMessage.ByteReader.Get<short>();

        List<Location> positions = new();

        while (input.DataMessage.ByteReader.BytesLeft > 0)
        {
            Location position = Position(input, id);

            positions.Add(position);
        }

        SendResponse(input, id);

        return positions;
    }

    private static void SendResponse(MessageInput input, long id)
    {
        string head = $"{0:X4}";
        string body = $"4CBF{id:X8}{0:X4}";
        string checksum = Crc.ComputeHash(HexUtil.ConvertHexStringToByteArray(body), CrcAlgorithm.Crc16CcittFalse);
        string response = $"{head}{body}{checksum}";

        input.NetworkStream.Write(HexUtil.ConvertHexStringToByteArray(response));
    }

    private static Location Position(MessageInput input, long id)
    {
        short checksum = input.DataMessage.ByteReader.Get<short>();
        short preamble = input.DataMessage.ByteReader.Get<short>();
        short type = input.DataMessage.ByteReader.Get<short>();
        int length = input.DataMessage.ByteReader.Get<short>();

        input.Client.SetDevice($"{id}");
            
        Location location = new()
        {
            Device = input.Client.Device
        };

        location.DateTime = DateTime.UnixEpoch.AddSeconds(input.DataMessage.ByteReader.Get<int>());
        location.Latitude = input.DataMessage.ByteReader.Get<int>() * 0.000001;
        location.Longitude = input.DataMessage.ByteReader.Get<int>() * 0.000001;
        location.Speed = input.DataMessage.ByteReader.Get<short>() * 0.01f;
        location.Altitude = input.DataMessage.ByteReader.Get<short>();
        location.Heading = input.DataMessage.ByteReader.Get<short>();
        location.PositionStatus = input.DataMessage.ByteReader.GetOne() > 0;
        location.Satellites = input.DataMessage.ByteReader.GetOne();

        byte @event = input.DataMessage.ByteReader.GetOne();

        input.DataMessage.ByteReader.GetOne();
        input.DataMessage.ByteReader.Skip(length);
            
        return location;
    }
}