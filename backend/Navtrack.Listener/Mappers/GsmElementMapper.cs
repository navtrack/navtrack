using Navtrack.DataAccess.Model.Positions;
using Navtrack.Listener.Models;

namespace Navtrack.Listener.Mappers;

public static class GsmElementMapper
{
    public static GsmElement? Map(Position source)
    {
        CellGlobalIdentityElement? cgi = CellGlobalIdentityElementMapper.Map(source);
        
        if (source.GsmSignal != null || cgi != null)
        {
            return new GsmElement
            {
                Signal = source.GsmSignal,
                CellGlobalIdentity = CellGlobalIdentityElementMapper.Map(source)
            };
        }

        return null;
    }
}