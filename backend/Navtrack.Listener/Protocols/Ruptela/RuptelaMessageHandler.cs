using System;
using System.Collections.Generic;
using System.Linq;
using Navtrack.Library.DI;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Helpers.Crc;
using Navtrack.Listener.Models;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.Ruptela;

[Service(typeof(ICustomMessageHandler<RuptelaProtocol>))]
public class RuptelaMessageHandler : BaseMessageHandler<RuptelaProtocol>
{
    public override IEnumerable<Location> ParseRange(MessageInput input)
    {
        short size = input.DataMessage.ByteReader.GetLe<short>();
        long imei = input.DataMessage.ByteReader.GetLe<long>();

        input.Client.SetDevice($"{imei}");

        byte command = input.DataMessage.ByteReader.GetOne();

        if (Enum.GetValues(typeof(Command)).Cast<int>().Contains(command))
        {
            byte recordsLeftInDevice = input.DataMessage.ByteReader.GetOne();
            byte records = input.DataMessage.ByteReader.GetOne();

            List<Location> locations = new();

            for (int i = 0; i < records; i++)
            {
                Location location = GetLocation(input, command == (int) Command.ExtendedRecords);

                locations.Add(location);
            }

            SendResponse(input);

            return locations;
        }

        return null;
    }

    private static void SendResponse(MessageInput input)
    {
        // ReSharper disable once PossiblyMistakenUseOfInterpolatedStringInsert
        string packetLength = $"{2:X4}";
        const string command = "64";
        const string ack = "01";
        string checksum = Crc.ComputeHash(HexUtil.ConvertHexStringToByteArray($"{command}{ack}"),
            CrcAlgorithm.Crc16Kermit);
        string fullReply = $"{packetLength}{command}{ack}{checksum}";

        input.NetworkStream.Write(HexUtil.ConvertHexStringToByteArray(fullReply));
    }

    private static Location GetLocation(MessageInput input, bool extended)
    {
        // ReSharper disable once UseObjectOrCollectionInitializer
        Location location = new()
        {
            Device = input.Client.Device
        };

        location.DateTime = DateTime.UnixEpoch.AddSeconds(input.DataMessage.ByteReader.GetLe<int>());
        byte timestampExtension = input.DataMessage.ByteReader.GetOne();
        byte? recordExtension = extended
            ? input.DataMessage.ByteReader.GetOne()
            : default;
        byte priority = input.DataMessage.ByteReader.GetOne();
        location.Longitude = input.DataMessage.ByteReader.GetLe<int>() / 10000000.0;
        location.Latitude = input.DataMessage.ByteReader.GetLe<int>() / 10000000.0;
        location.Altitude = input.DataMessage.ByteReader.GetLe<short>() / 10f;
        location.Heading = input.DataMessage.ByteReader.GetLe<short>() / 100f;
        location.Satellites = input.DataMessage.ByteReader.GetOne();
        location.Speed = input.DataMessage.ByteReader.GetLe<short>();
        location.HDOP = input.DataMessage.ByteReader.GetOne() / 10;
        short eventId = extended
            ? input.DataMessage.ByteReader.GetLe<short>()
            : input.DataMessage.ByteReader.GetOne();

        List<IOData> events = new();

        events.AddRange(GetIOData(input.DataMessage.ByteReader, 1, extended)); // 1 byte IO data
        events.AddRange(GetIOData(input.DataMessage.ByteReader, 2, extended)); // 2 bytes IO data
        events.AddRange(GetIOData(input.DataMessage.ByteReader, 4, extended)); // 4 bytes IO data
        events.AddRange(GetIOData(input.DataMessage.ByteReader, 8, extended)); // 8 bytes IO data

        return location;
    }

    private static IEnumerable<IOData> GetIOData(ByteReader input, int eventBytes, bool extended)
    {
        List<IOData> data = new();

        byte ioDataCount = input.GetOne();

        for (int i = 0; i < ioDataCount; i++)
        {
            data.Add(new IOData
            {
                Id = extended ? input.GetLe<short>() : input.GetOne(),
                Value = input.Get(eventBytes)
            });
        }

        return data;
    }
}