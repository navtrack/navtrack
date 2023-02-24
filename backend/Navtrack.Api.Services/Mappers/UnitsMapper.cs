using Navtrack.Core.Model;
using Navtrack.DataAccess.Model.Common;

namespace Navtrack.Api.Services.Mappers;

public static class UnitsMapper
{
    public static float? MapSpeed(float? speed, UnitsType unitsType)
    {
        return speed.HasValue ? unitsType == UnitsType.Metric ? speed : speed.Value / 1.609f : null;
    }

    public static float? MapDistance(float? distance, UnitsType unitsType)
    {
        return distance.HasValue ? unitsType == UnitsType.Metric ? distance : distance.Value * 3.281f : null;
    }
}