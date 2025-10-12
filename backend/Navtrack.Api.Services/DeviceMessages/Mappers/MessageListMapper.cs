using System.Linq;
using Navtrack.Api.Model.Messages;
using Navtrack.Database.Model.Filters;

namespace Navtrack.Api.Services.DeviceMessages.Mappers;

public static class MessageListMapper
{
    public static MessageList Map(GetMessagesResult source)
    {
        MessageList result = new()
        {
            Items = source.Messages.Select(MessageMapper.Map).ToList(),
            TotalCount = source.TotalCount
        };

        return result;
    }
}