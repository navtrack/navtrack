using System;
using Navtrack.Database.Model.Devices;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.Neomatica;

[Service(typeof(ICustomMessageHandler<NeomaticaProtocol>))]
public class NeomaticaMessageHandler : BaseMessageHandler<NeomaticaProtocol>
{
    public override DeviceMessageEntity? Parse(MessageInput input)
    {
        input.DataMessage.ByteReader.Get<short>(); // device id

        int size = input.DataMessage.ByteReader.GetOne();
        int type = input.DataMessage.ByteReader.GetOne();

        if (type == 0x03) // imei
        {
            input.ConnectionContext.SetDevice(StringUtil.ConvertByteArrayToString(input.DataMessage.ByteReader.Get(15)));
        }
        else
        {
            return GetLocation(input, type);
        }

        return null;
    }


    private static DeviceMessageEntity? GetLocation(MessageInput input, int type)
    {
        if (input.ConnectionContext.Device == null)
        {
            return null;
        }

        DeviceMessageEntity deviceMessage = new();
        
        input.DataMessage.ByteReader.GetOne();
        input.DataMessage.ByteReader.Get<short>();

        short status = input.DataMessage.ByteReader.Get<short>();
        deviceMessage.Valid = !BitUtil.IsTrue(status, 5);
        deviceMessage.Latitude = input.DataMessage.ByteReader.Get<float>();
        deviceMessage.Longitude = input.DataMessage.ByteReader.Get<float>();
        deviceMessage.Heading = (input.DataMessage.ByteReader.Get<short>() * 0.1f).ToShort();
        deviceMessage.Speed = (input.DataMessage.ByteReader.Get<short>() * 0.1f).ToShort();
        input.DataMessage.ByteReader.GetOne();
        deviceMessage.Altitude = input.DataMessage.ByteReader.Get<short>();
        deviceMessage.HDOP = input.DataMessage.ByteReader.GetOne() * 0.1f;
        deviceMessage.Satellites = (short?)(input.DataMessage.ByteReader.GetOne() & 0x0f);
        deviceMessage.Date = DateTime.UnixEpoch.AddSeconds(input.DataMessage.ByteReader.Get<int>());

        MarkAsNull(deviceMessage);

        return deviceMessage;
    }

    private static void MarkAsNull(DeviceMessageEntity deviceMessageDocument)
    {
        if (!deviceMessageDocument.Valid.GetValueOrDefault())
        {
            deviceMessageDocument.Heading = null;
            deviceMessageDocument.Speed = null;
            deviceMessageDocument.Altitude = null;
            deviceMessageDocument.HDOP = null;
            deviceMessageDocument.Satellites = null;
        }
    }
}