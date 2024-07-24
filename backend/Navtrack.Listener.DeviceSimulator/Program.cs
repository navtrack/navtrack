using System;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Navtrack.Listener.Helpers.New;

namespace Navtrack.Listener.DeviceSimulator;

class Program
{
    static void Main(string[] args)
    {
        SimulateMeitrack();
    }

    private static void SimulateMeitrack()
    {
        TcpClient tcpClient = new();
        tcpClient.Connect(IPAddress.Loopback, 7001);
        NetworkStream networkStream = tcpClient.GetStream();

        while (true)
        {
            try
            {
                string date = DateTime.UtcNow.ToString("yyMMddHHmmss");

                (double, double) location = GetRandomLocation();

                int altitude = new Random().Next(0, 2000);
                int heading = new Random().Next(0, 360);

                string text =
                    $"$$F142,123456789012345,AAA,35,{location.Item1.ToString("F6", CultureInfo.InvariantCulture)},{location.Item2.ToString("F6", CultureInfo.InvariantCulture)},{date},A,5,30,0,{heading},2.5,{altitude},56364283,8983665,123|4|0000|0000,0421,0200|000E||02EF|00FC,*";

                string checksum = ChecksumUtil.Xor(Encoding.UTF8.GetBytes(text));
                text += $"{checksum}\r\n";
            
                Console.Write(text);
            
                networkStream.Write(Encoding.UTF8.GetBytes(text));
            }
            catch (Exception e)
            {
                // ignored
            }
          
            Thread.Sleep(5000);
        }
    }

    private static (double, double) GetRandomLocation()
    {
        Random rand = new();

        // Generate a random latitude between 46.7662 and 46.7781 degrees (the latitude range of the center of Cluj-Napoca)
        double lat = rand.NextDouble() * 0.0119 + 46.7662;

        // Generate a random longitude between 23.5896 and 23.6162 degrees (the longitude range of the center of Cluj-Napoca)
        double lon = rand.NextDouble() * 0.0266 + 23.5896;

        return (lat, lon);
    }
}