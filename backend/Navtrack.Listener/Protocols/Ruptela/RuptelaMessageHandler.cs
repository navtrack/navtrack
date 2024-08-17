using System;
using System.Collections.Generic;
using System.Linq;
using Navtrack.DataAccess.Model.Devices.Messages;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Helpers.Crc;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.Ruptela;

[Service(typeof(ICustomMessageHandler<RuptelaProtocol>))]
public class RuptelaMessageHandler : BaseMessageHandler<RuptelaProtocol>
{
    public override IEnumerable<DeviceMessageDocument>? ParseRange(MessageInput input)
    {
        short size = input.DataMessage.ByteReader.GetLe<short>();
        long imei = input.DataMessage.ByteReader.GetLe<long>();

        input.ConnectionContext.SetDevice($"{imei}");

        byte command = input.DataMessage.ByteReader.GetOne();

        if (Enum.GetValues(typeof(Command)).Cast<int>().Contains(command))
        {
            byte recordsLeftInDevice = input.DataMessage.ByteReader.GetOne();
            byte records = input.DataMessage.ByteReader.GetOne();

            List<DeviceMessageDocument> locations = [];

            for (int i = 0; i < records; i++)
            {
                DeviceMessageDocument deviceMessageDocument = GetLocation(input, command == (int) Command.ExtendedRecords);

                locations.Add(deviceMessageDocument);
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

    private static DeviceMessageDocument GetLocation(MessageInput input, bool extended)
    {
        // ReSharper disable once UseObjectOrCollectionInitializer
        DeviceMessageDocument deviceMessageDocument = new()
        {
            // Device = input.ConnectionContext.Device,
            Position = new PositionElement()
        };

        deviceMessageDocument.Position.Date = DateTime.UnixEpoch.AddSeconds(input.DataMessage.ByteReader.GetLe<int>());
        byte timestampExtension = input.DataMessage.ByteReader.GetOne();
        byte? recordExtension = extended
            ? input.DataMessage.ByteReader.GetOne()
            : default;
        byte priority = input.DataMessage.ByteReader.GetOne();
        deviceMessageDocument.Position.Longitude = input.DataMessage.ByteReader.GetLe<int>() / 10000000.0;
        deviceMessageDocument.Position.Latitude = input.DataMessage.ByteReader.GetLe<int>() / 10000000.0;
        deviceMessageDocument.Position.Altitude = input.DataMessage.ByteReader.GetLe<short>() / 10f;
        deviceMessageDocument.Position.Heading = input.DataMessage.ByteReader.GetLe<short>() / 100f;
        deviceMessageDocument.Position.Satellites = input.DataMessage.ByteReader.GetOne();
        deviceMessageDocument.Position.Speed = input.DataMessage.ByteReader.GetLe<short>();
        deviceMessageDocument.Position.HDOP = input.DataMessage.ByteReader.GetOne() / 10;
        short eventId = extended
            ? input.DataMessage.ByteReader.GetLe<short>()
            : input.DataMessage.ByteReader.GetOne();

        List<IOData> events = [];

        events.AddRange(GetIOData(input.DataMessage.ByteReader, 1, extended)); // 1 byte IO data
        events.AddRange(GetIOData(input.DataMessage.ByteReader, 2, extended)); // 2 bytes IO data
        events.AddRange(GetIOData(input.DataMessage.ByteReader, 4, extended)); // 4 bytes IO data
        events.AddRange(GetIOData(input.DataMessage.ByteReader, 8, extended)); // 8 bytes IO data

        return deviceMessageDocument;
    }

    private static IEnumerable<IOData> GetIOData(ByteReader input, int eventBytes, bool extended)
    {
        List<IOData> data = [];

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