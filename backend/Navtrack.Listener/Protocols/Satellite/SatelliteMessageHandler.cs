using System;
using System.Collections.Generic;
using Navtrack.Database.Model.Devices;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Helpers.Crc;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.Satellite;

[Service(typeof(ICustomMessageHandler<SatelliteProtocol>))]
public class SatelliteMessageHandler : BaseMessageHandler<SatelliteProtocol>
{
    public override IEnumerable<DeviceMessageEntity>? ParseRange(MessageInput input)
    {
        short checksum = input.DataMessage.ByteReader.Get<short>();
        short preamble = input.DataMessage.ByteReader.Get<short>();
        long id = input.DataMessage.ByteReader.Get<int>();
        short length = input.DataMessage.ByteReader.Get<short>();

        List<DeviceMessageEntity> positions = [];

        while (input.DataMessage.ByteReader.BytesLeft > 0)
        {
            DeviceMessageEntity deviceMessageDocument = Position(input, id);

            positions.Add(deviceMessageDocument);
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

    private static DeviceMessageEntity Position(MessageInput input, long id)
    {
        short checksum = input.DataMessage.ByteReader.Get<short>();
        short preamble = input.DataMessage.ByteReader.Get<short>();
        short type = input.DataMessage.ByteReader.Get<short>();
        int length = input.DataMessage.ByteReader.Get<short>();

        input.ConnectionContext.SetDevice($"{id}");

        DeviceMessageEntity deviceMessageDocument = new();

        deviceMessageDocument.Date = DateTime.UnixEpoch.AddSeconds(input.DataMessage.ByteReader.Get<int>());
        deviceMessageDocument.Latitude = input.DataMessage.ByteReader.Get<int>() * 0.000001;
        deviceMessageDocument.Longitude = input.DataMessage.ByteReader.Get<int>() * 0.000001;
        deviceMessageDocument.Speed = (input.DataMessage.ByteReader.Get<short>() * 0.01f).ToShort();
        deviceMessageDocument.Altitude = input.DataMessage.ByteReader.Get<short>();
        deviceMessageDocument.Heading = input.DataMessage.ByteReader.Get<short>();
        deviceMessageDocument.Valid = input.DataMessage.ByteReader.GetOne() > 0;
        deviceMessageDocument.Satellites = input.DataMessage.ByteReader.GetOne();

        byte @event = input.DataMessage.ByteReader.GetOne();

        input.DataMessage.ByteReader.GetOne();
        input.DataMessage.ByteReader.Skip(length);
            
        return deviceMessageDocument;
    }
}