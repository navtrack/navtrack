using Navtrack.Listener.Protocols.GlobalSat;
using NUnit.Framework;

namespace Navtrack.Listener.Tests.Protocols.GlobalSat;

public class GlobalSatProtocolTests : BaseProtocolTests<GlobalSatProtocol, GlobalSatMessageHandler>
{
    [Test]
    public void DeviceSendsLocationV1_LocationIsParsed()
    {
        ProtocolTester.SendStringFromDevice(
            "GSr,135785412249986,01,I,EA02,,3,230410,153318,E12129.2839,N2459.8570,0,1.17,212,8,1.0,12.3V*55!");

        Assert.IsNotNull(ProtocolTester.LastParsedLocation);
    }

    [Test]
    public void DeviceSendsLocationV2_LocationIsParsed()
    {
        ProtocolTester.SendStringFromDevice(
            "GSb,GTR-388,358173053992353,0000,5,8080,3,270419,113326,E01020.6223,N6323.1937,129,0.01,154,10,0.8,12380mV,3128mV,0,0,11,242,02,10EB,120FC1B*5a!");

        Assert.IsNotNull(ProtocolTester.LastParsedLocation);
    }

    [Test]
    public void DeviceSendsLocationV3_LocationIsParsed()
    {
        ProtocolTester.SendStringFromDevice(
            "GSr,011412001878820,4,5,00,,1,250114,105316,E00610.2925,N4612.1824,0,0.02,0,1,0.0,64*51!");

        Assert.IsNotNull(ProtocolTester.LastParsedLocation);
    }
        
    [Test]
    public void DeviceSendsLocationV4_LocationIsParsed()
    {
        ProtocolTester.SendStringFromDevice(
            "GSr,357938020310710,,4,04,,1,170315,060657,E00000.0000,N0000.0000,148,0.00,0,0,0.0,11991mV*6c!");

        Assert.IsNotNull(ProtocolTester.LastParsedLocation);
    }
        
    [Test]
    public void DeviceSendsLocationV5_LocationIsParsed()
    {
        ProtocolTester.SendStringFromDevice(
            "GSr,1,135785412249986,01,I,EA02,3,230410,153318,E12129.2839,N2459.8570,0,1.17,212,8,1.0,12.3V*55");

        Assert.IsNotNull(ProtocolTester.LastParsedLocation);
    }

    [Test]
    public void DeviceSendsLocationV6_LocationIsParsed()
    {
        ProtocolTester.SendStringFromDevice(
            "$355632004245866,1,1,040202,093633,E12129.2252,N2459.8891,00161,0.0100,147,07,2.4");

        Assert.IsNotNull(ProtocolTester.LastParsedLocation);
    }

    [Test]
    public void DeviceSendsLocationV7_LocationIsParsed()
    {
        ProtocolTester.SendStringFromDevice(
            "$353681041893264,9,3,240913,100833,E08513.0122,N5232.9395,181.3,22.02,251.30,9,1.00");

        Assert.IsNotNull(ProtocolTester.LastParsedLocation);
    }

    [Test]
    public void DeviceSendsLocationV8_LocationIsParsed()
    {
        ProtocolTester.SendStringFromDevice(
            "$80050377796567,0,13,281015,173437,E08513.28616,N5232.85432,222.3,0.526,,07*37");

        Assert.IsNotNull(ProtocolTester.LastParsedLocation);
    }

    [Test]
    public void DeviceSendsLocationV9_LocationIsParsed()
    {
        ProtocolTester.SendStringFromDevice(
            "$80050377796567,0,18,281015,191919,E08513.93290,N5232.42141,193.4,37.647,305.40,07*37");

        Assert.IsNotNull(ProtocolTester.LastParsedLocation);
    }
}