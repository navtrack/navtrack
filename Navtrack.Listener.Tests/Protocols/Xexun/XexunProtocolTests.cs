using Navtrack.Listener.Protocols.Xexun;
using NUnit.Framework;

namespace Navtrack.Listener.Tests.Protocols.Xexun;

public class XexunProtocolTests : BaseProtocolTests<XexunProtocol, XexunMessageHandler>
{
    [Test]
    public void DeviceSendsLocationV1_LocationIsParsed()
    {
        ProtocolTester.SendStringFromDevice("090805215127,+22663173122,GPRMC,215127.083,A,4717.3044,N,01135.0005,E,0.39,217.95,050809,,,A*6D,F,, imei:354776030393299,05,552.4,F:4.06V,0,141,54982,232,01,1A30,0949");

        Assert.IsNotNull(ProtocolTester.LastParsedLocation);
    }
        
    [Test]
    public void DeviceSendsLocationV2_LocationIsParsed()
    {
        ProtocolTester.SendStringFromDevice("0711011832,+8613145826126,GPRMC,103226.000,A,2234.0239,N,11403.0765,E,0.00,,011107,,,A*7E,F,imei:352022008228783,101j");

        Assert.IsNotNull(ProtocolTester.LastParsedLocation);
    }
}