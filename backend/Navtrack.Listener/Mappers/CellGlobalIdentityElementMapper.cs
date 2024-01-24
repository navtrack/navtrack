using Navtrack.DataAccess.Model.New;
using Navtrack.Listener.Models;

namespace Navtrack.Listener.Mappers;

public static class CellGlobalIdentityElementMapper
{
    public static NewCellGlobalIdentityElement? Map(Location source)
    {
        if (source.MobileCountryCode.HasValue || source.MobileNetworkCode.HasValue ||
            source.LocationAreaCode.HasValue || source.CellId.HasValue)
        {
            return new NewCellGlobalIdentityElement
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