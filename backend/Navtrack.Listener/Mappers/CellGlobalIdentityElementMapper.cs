using Navtrack.DataAccess.Model.Positions;
using Navtrack.Listener.Models;

namespace Navtrack.Listener.Mappers;

public static class CellGlobalIdentityElementMapper
{
    public static CellGlobalIdentityElement? Map(Position source)
    {
        if (source.MobileCountryCode.HasValue || source.MobileNetworkCode.HasValue ||
            source.LocationAreaCode.HasValue || source.CellId.HasValue)
        {
            return new CellGlobalIdentityElement
            {
                MobileCountryCode = source.MobileCountryCode,
                MobileNetworkCode = source.MobileNetworkCode,
                LocationAreaCode = source.LocationAreaCode,
                CellId = source.CellId,
            };
        }

        return null;
    }
}