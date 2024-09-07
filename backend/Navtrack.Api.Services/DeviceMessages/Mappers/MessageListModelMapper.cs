using System.Linq;
using Navtrack.Api.Model.Messages;
using Navtrack.DataAccess.Model.Devices.Messages.Filters;

namespace Navtrack.Api.Services.DeviceMessages.Mappers;

public static class MessageListModelMapper
{
    public static MessageListModel Map(GetMessagesResult source)
    {
        MessageListModel model = new()
        {
            Items = source.Messages.Select(MessageModelMapper.Map).ToList(),
            TotalCount = source.TotalCount
        };

        return model;
    }
}