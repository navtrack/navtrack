using System.Linq;
using Navtrack.Api.Model.Positions;
using Navtrack.DataAccess.Services.Positions;

namespace Navtrack.Api.Services.Mappers.Positions;

public static class PositionListMapper
{
    public static PositionListModel Map(GetPositionsResult source)
    {
        PositionListModel model = new()
        {
            Items = source.Positions.Select(PositionMapper.Map).ToList(),
            TotalCount = source.TotalCount
        };

        return model;
    }
}