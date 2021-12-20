using Navtrack.Listener.Protocols.CarTrackGPS;
using NUnit.Framework;

namespace Navtrack.Listener.Tests.Protocols.CarTrackGPS;

public class CarTrackGPSProtocolTests : BaseProtocolTests<CarTrackGPSProtocol, CarTrackGPSMessageHandler>
{
    [Test]
    public void DeviceSendsLocationV1_LocationIsParsed()
    {
        ProtocolTester.SendStringFromDevice(
            "$$020040????????&A9955&B011939.000,A,4436.3804,N,02606.9434,E,0.00,0.00,190317,,,A*64|0.9|&C0100000000&D01830=?6&E00000001&Y00000000");

        Assert.IsNotNull(ProtocolTester.LastParsedLocation);
    }

    [Test]
    public void DeviceSendsLocationV2_LocationIsParsed()
    {
        ProtocolTester.SendStringFromDevice(
            "$$2222234???????&A9955&B102904.000,A,2233.0655,N,11404.9440,E,0.00,,030109,,*17|6.3|&C0100000100&D000024?>&E10000000");

        Assert.IsNotNull(ProtocolTester.LastParsedLocation);
    }

    [Test]
    public void DeviceSendsLocationV3_LocationIsParsed()
    {
        ProtocolTester.SendStringFromDevice(
            "$$2222234???????&A9955&B102904.000,A,2233.0655,N,11404.9440,E,0.00,,030109,,*17|6.3|&C0100000100&D000024?>&E10000000&Y00100020");

        Assert.IsNotNull(ProtocolTester.LastParsedLocation);
    }
        
    [Test]
    public void DeviceSendsLocationV4_LocationIsParsed()
    {
        ProtocolTester.SendStringFromDevice(
            "$$2222234???????&A9955&B102904.000,A,2233.0655,N,11404.9440,E,0.00,,030109,,*17|6.3|&C0100000100&D000024?>&E10000000");

        Assert.IsNotNull(ProtocolTester.LastParsedLocation);
    }
}