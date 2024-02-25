using System.Collections.Generic;

namespace Navtrack.DataAccess.Model.Positions;

public class GetPositionsResult
{
    public required long TotalCount { get; init; }
    public required List<PositionDocument> Positions { get; init; }
}