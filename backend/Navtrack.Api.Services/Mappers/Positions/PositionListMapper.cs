using System.Linq;
using Navtrack.Api.Model.Positions;
using Navtrack.DataAccess.Model.Positions;

namespace Navtrack.Api.Services.Mappers.Positions;

public static class PositionListMapper
{
    public static PositionListModel Map(GetMessagesResult source)
    {
        PositionListModel model = new()
        {
            Items = source.Messages.Select(PositionModelMapper.Map).ToList(),
            TotalCount = source.TotalCount
        };

        return model;
    }
}