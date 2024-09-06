using System;
using System.Collections.Generic;
using Navtrack.DataAccess.Model.Devices.Messages;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.Galileosky;

[Service(typeof(ICustomMessageHandler<GalileoskyProtocol>))]
public class GalileoskyMessageHandler : BaseMessageHandler<GalileoskyProtocol>
{
    public override IEnumerable<DeviceMessageDocument>? ParseRange(MessageInput input)
    {
        input.DataMessage.ByteReader.Skip(1);
        int length = input.DataMessage.ByteReader.Get<short>() & 0x7fff;

        List<DeviceMessageDocument> positions = [];
        DeviceMessageDocument deviceMessageDocument = new();
        deviceMessageDocument.Position = new PositionElement();

        HashSet<byte> tagsFound = [];

        while (input.DataMessage.ByteReader.Index < input.DataMessage.Bytes.Length - 2)
        {
            byte tag = input.DataMessage.ByteReader.GetOne();

            if (tagsFound.Contains(tag))
            {
                deviceMessageDocument = new DeviceMessageDocument
                {
                    Position = new PositionElement()
                };
                tagsFound.Clear();
            }

            tagsFound.Add(tag);
            int? tagLength = GalileoskyProtocolTags.GetTagLength(tag);

            switch (tag)
            {
                case 0x03:
                    byte[] bytes = input.DataMessage.ByteReader.Get(tagLength.GetValueOrDefault());
                    input.ConnectionContext.SetDevice(StringUtil.ConvertByteArrayToString(
                        bytes));
                    break;

                case 0x20:
                    deviceMessageDocument.Position.Date =
                        DateTime.UnixEpoch.AddSeconds(input.DataMessage.ByteReader.Get<int>());
                    break;

                case 0x30:
                    deviceMessageDocument.Position.Valid = (input.DataMessage.ByteReader.GetOne() & 0xf0) == 0x00;
                    deviceMessageDocument.Position.Latitude = input.DataMessage.ByteReader.Get<int>() / 1000000.0;
                    deviceMessageDocument.Position.Longitude = input.DataMessage.ByteReader.Get<int>() / 1000000.0;
                    positions.Add(deviceMessageDocument);
                    break;

                case 0x33:
                    deviceMessageDocument.Position.Speed = input.DataMessage.ByteReader.Get<short>() / 10;
                    deviceMessageDocument.Position.Heading = input.DataMessage.ByteReader.Get<short>() / 10;
                    break;

                case 0x34:
                    deviceMessageDocument.Position.Altitude = input.DataMessage.ByteReader.Get<short>();
                    break;

                case 0xE1:
                    input.DataMessage.ByteReader.Skip(input.DataMessage.ByteReader.GetOne());
                    break;

                default:
                    input.DataMessage.ByteReader.Skip(tagLength.GetValueOrDefault());
                    break;
            }
        }

        string checksum = input.DataMessage.Hex[^2..].StringJoin();
        input.NetworkStream.Write(HexUtil.ConvertHexStringToByteArray($"02{checksum}"));

        // positions.ForEach(x => x.Device = input.ConnectionContext.Device);

        return positions;
    }
}