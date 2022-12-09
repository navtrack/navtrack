using Navtrack.Listener.Protocols.Megastek;
using NUnit.Framework;

namespace Navtrack.Listener.Tests.Protocols.Megastek;

public class MegastekProtocolTests : BaseProtocolTests<MegastekProtocol, MegastekMessageHandler>
{
    [Test]
    public void DeviceSendsLocationV1_ParsedLocationIsNotNull()
    {
        ProtocolTester.SendHexFromDevice("4C 4F 47 53 54 58 2C 31 30 32 31 31 30 38 33 30 30 37 34 35 34 32 2C 24 47 50 52 4D 43 2C 31 31 34 32 32 39 2E 30 30 30 2C 41 2C 32 32 33 38 2E 32 30 32 34 2C 4E 2C 31 31 34 30 31 2E 39 36 31 39 2C 45 2C 30 2E 30 30 2C 30 2E 30 30 2C 33 31 30 38 31 31 2C 2C 2C 41 2A 36 34 2C 46 2C 4C 6F 77 42 61 74 74 65 72 79 2C 69 6D 65 69 3A 30 31 32 32 30 37 30 30 35 35 35 33 38 38 35 2C 30 33 2C 31 31 33 2E 31 2C 42 61 74 74 65 72 79 3D 32 34 25 2C 2C 31 2C 34 36 30 2C 30 31 2C 32 35 33 31 2C 36 34 37 45 3B 35 37 0D 0A");

        Assert.IsNotNull(ProtocolTester.LastParsedLocation);
    }
        
    [Test]
    public void DeviceSendsLocationV2_ParsedLocationIsNotNull()
    {
        ProtocolTester.SendHexFromDevice("53 54 58 2C 31 30 32 31 31 30 38 33 30 30 37 34 35 34 32 2C 24 47 50 52 4D 43 2C 31 31 34 32 32 39 2E 30 30 30 2C 41 2C 32 32 33 38 2E 32 30 32 34 2C 4E 2C 31 31 34 30 31 2E 39 36 31 39 2C 45 2C 30 2E 30 30 2C 30 2E 30 30 2C 33 31 30 38 31 31 2C 2C 2C 41 2A 36 34 2C 46 2C 4C 6F 77 42 61 74 74 65 72 79 2C 69 6D 65 69 3A 30 31 32 32 30 37 30 30 35 35 35 33 38 38 35 2C 30 33 2C 31 31 33 2E 31 2C 42 61 74 74 65 72 79 3D 32 34 25 2C 2C 31 2C 34 36 30 2C 30 31 2C 32 35 33 31 2C 36 34 37 45 3B 35 37 0D 0A");

        Assert.IsNotNull(ProtocolTester.LastParsedLocation);
    }

    [Test]
    public void DeviceSendsLocationV3_ParsedLocationIsNotNull()
    {
        ProtocolTester.SendHexFromDevice("53 54 58 31 32 33 34 35 36 20 20 20 20 20 20 20 20 20 20 02 7D 24 47 50 52 4D 43 2C 30 36 33 37 30 39 2E 30 30 30 2C 41 2C 32 32 33 38 2E 31 39 39 38 2C 4E 2C 31 31 34 30 31 2E 39 36 37 30 2C 45 2C 30 2E 30 30 2C 2C 32 35 30 33 31 33 2C 2C 2C 41 2A 37 46 2C 34 36 30 2C 30 31 2C 32 35 33 31 2C 36 34 37 45 2C 31 31 2C 38 37 2C 31 30 30 30 2C 30 30 31 30 30 31 2C 30 30 30 30 2C 30 2E 30 30 2C 30 2E 30 32 2C 30 2E 30 30 2C 54 69 6D 65 72 3B 34 41 0D 0A");

        Assert.IsNotNull(ProtocolTester.LastParsedLocation);
    }

    [Test]
    public void DeviceSendsLocationV4_ParsedLocationIsNotNull()
    {
        ProtocolTester.SendStringFromDevice(
            "0125$MGV002,860719020193193,DeviceName,R,240214,104742,A,2238.20471,N,11401.97967,E,00,03,00,1.20,0.462,356.23,137.9,1.5,460,07,262C,0F54,25,0000,0000,0,0,0,28.5,28.3,,,100,Timer;!");

        Assert.IsNotNull(ProtocolTester.LastParsedLocation);
    }
        
    [Test]
    public void DeviceSendsLocationV5_ParsedLocationIsNotNull()
    {
        ProtocolTester.SendStringFromDevice(
            "$MGV002,860719020193193,DeviceName,R,240214,104742,A,2238.20471,N,11401.97967,E,00,03,00,1.20,0.462,356.23,137.9,1.5,460,07,262C,0F54,25,0000,0000,0,0,0,28.5,28.3,,10,100,Timer;!");

        Assert.IsNotNull(ProtocolTester.LastParsedLocation);
    }
        
    [Test]
    public void DeviceSendsLocationV6_ParsedLocationIsNotNull()
    {
        ProtocolTester.SendStringFromDevice(
            "0180$MGV002,860719020193193,DeviceName,R,240214,104742,A,2238.20471,N,11401.97967,E,00,03,00,1.20,0.462,356.23,137.9,1.5,460,07,262C,0F54,25,0000,0000,523,0,0,28.5,28.3,,1,100,Timer;!");

        Assert.IsNotNull(ProtocolTester.LastParsedLocation);
    }
        
    [Test]
    public void DeviceSendsLocationV7_ParsedLocationIsNotNull()
    {
        ProtocolTester.SendStringFromDevice(
            "0645$MGV002,860719020193193,DeviceName,R,240214,104742,A,2238.20471,N,11401.97967,E,00,03,00,1.20,0.462,356.23,137.9,1.5,460,07,262C,0F54,25,0000,0000,0,0,0,28.5,28.3,,,100,Timer;!");

        Assert.IsNotNull(ProtocolTester.LastParsedLocation);
    }
        
    [Test]
    public void DeviceSendsLocationV8_ParsedLocationIsNotNull()
    {
        ProtocolTester.SendStringFromDevice(
            "0339$MGV002,860719020193193,DeviceName,R,240214,104742,A,2238.20471,N,11401.97967,E,00,03,00,1.20,0.462,356.23,137.9,1.5,460,07,262C,0F54,25,0000,0000,0,0,0,28.5,28.3,,10,100,Timer,18339df945d0:53|108c0fb0a2f1:57|e46f133d6f5c:59|108ccf109f21:59|8adc963d752a:82|04c5a48cc6c0:82|9adc963d752a:83|8800b0b00004:85|90671c80e2fc:85|80c5e68c8d36:86,;!");

        Assert.IsNotNull(ProtocolTester.LastParsedLocation);
    }
}