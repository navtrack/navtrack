using Navtrack.Listener.Protocols.Gotop;
using NUnit.Framework;

namespace Navtrack.Listener.Tests.Protocols.Gotop;

public class SanavProtocolTests : BaseProtocolTests<GotopProtocol, GotopMessageHandler>
{
    [Test]
    public void DeviceSendsLocationV1_LocationIsParsed()
    {
        ProtocolTester.SendStringFromDevice(
            "#353327020115804,CMD-T,A,DATE:090329,TIME:223252,LAT:22.7634066N,LOT:114.3964783E,Speed:000.0,84-20,000#");

        Assert.IsNotNull(ProtocolTester.LastParsedLocation);
    }

    [Test]
    public void DeviceSendsLocationV2_LocationIsParsed()
    {
        ProtocolTester.SendStringFromDevice(
            "#353327020115804,CMD-T,A,DATE:090329,TIME:223252,LAT:22.7634066N,LOT:114.3964783E,Speed:000.0,1-1-0-84-20,000#");

        Assert.IsNotNull(ProtocolTester.LastParsedLocation);
    }

    [Test]
    public void DeviceSendsLocationV3_LocationIsParsed()
    {
        ProtocolTester.SendStringFromDevice(
            "013949008891817,CMD-F,A,DATE:150225,TIME:175441,LAT:50.000000N,LOT:008.000000E,Speed:085.9,0-0-0-0-52-31,000,26201-1073-1DF5");

        Assert.IsNotNull(ProtocolTester.LastParsedLocation);
    }
        
    [Test]
    public void DeviceSendsLocationV4_LocationIsParsed()
    {
        ProtocolTester.SendStringFromDevice(
            "013226009991924,CMD-T,A,DATE:130802,TIME:153721,LAT:25.9757433S,LOT:028.1087816E,Speed:000.0,X-X-X-X-81-26,000,65501-00A0-4B8E");

        Assert.IsNotNull(ProtocolTester.LastParsedLocation);
    }
}