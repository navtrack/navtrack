using Navtrack.Listener.Helpers;
using Navtrack.Listener.Models;
using Xunit;

namespace Navtrack.Listener.Tests.Helpers;

public class GpsUtilTests
{
    [Fact]
    public void ConvertDdmToDecimal_Latitude()
    {
        const int degrees = 46;
        const double minutes = 46.272;
        const CardinalPoint cardinalPoint = CardinalPoint.North;

        double result = GpsUtil.ConvertDdmToDecimal(degrees, minutes, cardinalPoint);
        
        Assert.Equal(46.7712, result);
    }
    
    [Fact]
    public void ConvertDdmToDecimal_Longitude()
    {
        const int degrees = 23;
        const double minutes = 37.416;
        const CardinalPoint cardinalPoint = CardinalPoint.East;

        double result = GpsUtil.ConvertDdmToDecimal(degrees, minutes, cardinalPoint);
        
        Assert.Equal(23.6236, result);
    }
}