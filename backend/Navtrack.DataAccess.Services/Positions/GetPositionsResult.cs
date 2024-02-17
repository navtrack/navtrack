using System.Collections.Generic;
using Navtrack.DataAccess.Model.Positions;

namespace Navtrack.DataAccess.Services.Positions;

public class GetPositionsResult
{
    public required long TotalCount { get; init; }
    public required List<PositionElement> Positions { get; init; }
}