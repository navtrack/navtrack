using System;
using System.Collections.Generic;
using Navtrack.Database.Model.Devices;

namespace Navtrack.Listener.Server;

public class BaseMessageHandler<T> : ICustomMessageHandler<T>
{
    public virtual DeviceMessageEntity? Parse(MessageInput input)
    {
        return null;
    }

    public virtual IEnumerable<DeviceMessageEntity>? ParseRange(MessageInput input)
    {
        try
        {
            DeviceMessageEntity? deviceMessageDocument = Parse(input);

            return deviceMessageDocument != null ? new[] {deviceMessageDocument} : null;
        }
        catch (Exception)
        {
            return null;
        }
    }

    protected DeviceMessageEntity? Parse(MessageInput input, params Func<MessageInput, DeviceMessageEntity?>[] parsers)
    {
        foreach (Func<MessageInput,DeviceMessageEntity?> parse in parsers)
        {
            try
            {
                input.DataMessage.Reader.Reset();
                input.DataMessage.ByteReader.Reset();
                DeviceMessageEntity? deviceMessage = parse(input);

                if (deviceMessage != null)
                {
                    return deviceMessage;
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        return null;
    }
        
    protected IEnumerable<DeviceMessageEntity>? ParseRange(MessageInput input, params Func<MessageInput, IEnumerable<DeviceMessageEntity>?>[] parsers)
    {
        foreach (Func<MessageInput,IEnumerable<DeviceMessageEntity>?> parse in parsers)
        {
            try
            {
                input.DataMessage.Reader.Reset();
                input.DataMessage.ByteReader.Reset();
                IEnumerable<DeviceMessageEntity>? deviceMessages = parse(input);

                if (deviceMessages != null)
                {
                    return deviceMessages;
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        return null;
    }
}