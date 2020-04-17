using System;
using System.Linq;
using System.Text.RegularExpressions;
using Navtrack.Listener.Helpers;

namespace Navtrack.Listener.Protocols.Meiligao
{
    public class MeiligaoDataMessage
    {
        private string message;
        private readonly string[] split;
        
        public MeiligaoDataMessage(int[] frame)
        {
            message = StringUtil.ConvertByteArrayToString(frame);
            split = message.Split("|");
        }

        public string GPRMC => split[0];
        public string[] GPRMCArray => GPRMC.Split(",");
        public double HDOP => double.Parse(split[1]);
        public double Altitude => double.Parse(split[2]);
        public string State => split.GetValueOrDefault(3);
        public string AD => split.GetValueOrDefault(4);
        public string BaseId => split.GetValueOrDefault(5);
        public string CSQ => split.GetValueOrDefault(6);
        public string Journey => split.GetValueOrDefault(7);
        
        
        
        public decimal Latitude => ConvertDegreeAngleToDouble("(\\d{2})(\\d{2}).(\\d{4})", GPRMCArray[2], GPRMCArray[3]);
        public decimal Longitude => ConvertDegreeAngleToDouble("(\\d{3})(\\d{2}).(\\d{4})", GPRMCArray[4], GPRMCArray[5]);
        public bool GpsValid => GPRMCArray[1] == "A";
        public double Speed => double.TryParse(GPRMCArray[6], out double result) ? result* 1.852 : default;
        public double? Heading => double.TryParse(GPRMCArray[7], out double result) ? result : default;

        public string Checksum => GPRMCArray[^1].Replace("*", "");
        public string ChecksumComputed
        {
            get
            {
                string msg = GPRMC.Substring(0, GPRMC.IndexOf("*", StringComparison.InvariantCultureIgnoreCase)+1);

                int checksum = 0x100 - msg.Aggregate(0, (s, b) => s + b) & 0xff & 0xFF;
             
                return checksum.ToString("X2");
            }
        }

        public bool ChecksumValid => Checksum == ChecksumComputed;

        public DateTime DateTime
        {
            get
            {
                GroupCollection time = new Regex("(\\d{2})(\\d{2})(\\d{2}).(\\d{2})").Matches(GPRMCArray[0])[0].Groups;
                GroupCollection date = new Regex("(\\d{2})(\\d{2})(\\d{2})").Matches(GPRMCArray[8])[0].Groups;

                return new DateTime(2000+int.Parse(date[3].Value), int.Parse(date[2].Value), int.Parse(date[1].Value),
                    int.Parse(time[1].Value), int.Parse(time[2].Value), int.Parse(time[3].Value),
                    int.Parse(time[4].Value));
            }
        }

        
        private static decimal ConvertDegreeAngleToDouble(string pattern, string point, string cardinalDirection)
        {
            MatchCollection matchCollection = new Regex(pattern).Matches(point);

            int multiplier = cardinalDirection == "S" || cardinalDirection == "W" ? -1 : 1;
            decimal degrees = decimal.Parse(matchCollection[0].Groups[1].Value);
            decimal minutes = decimal.Parse(matchCollection[0].Groups[2].Value) / 60;
            decimal seconds = decimal.Parse(matchCollection[0].Groups[3].Value) / 3600;

            return Math.Round((degrees + minutes + seconds) * multiplier, 6, MidpointRounding.ToZero);
        }
    }
}