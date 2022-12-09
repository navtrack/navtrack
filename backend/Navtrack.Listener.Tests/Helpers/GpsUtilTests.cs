using Navtrack.Listener.Helpers;
using Navtrack.Listener.Models;
using NUnit.Framework;

namespace Navtrack.Listener.Tests.Helpers;

public class GpsUtilTests
{
    [Test]
    public void ConvertDdmToDecimal_Latitude()
    {
        
        
        
        
        
        
        
        
        const int degrees = 46;
        const double minutes = 46.272;
        const CardinalPoint cardinalPoint = CardinalPoint.North;

        double result = GpsUtil.ConvertDdmToDecimal(degrees, minutes, cardinalPoint);
        
        Assert.AreEqual(result, 46.7712);
    }
    
    [Test]
    public void ConvertDdmToDecimal_Longitude()
    {
        const int degrees = 23;
        const double minutes = 37.416;
        const CardinalPoint cardinalPoint = CardinalPoint.East;

        double result = GpsUtil.ConvertDdmToDecimal(degrees, minutes, cardinalPoint);
        
        Assert.AreEqual(result, 23.6236);
    }
}