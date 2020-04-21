using System;
using System.Linq;
using System.Text.RegularExpressions;
using Navtrack.Listener.Helpers;

namespace Navtrack.Listener.Protocols.Meiligao
{
    public class MeiligaoDataMessage
    {
        private readonly string[] split;
        
        public MeiligaoDataMessage(byte[] frame)
        {
            split = StringUtil.ConvertByteArrayToString(frame).Split("|");
        }

        public string GPRMC => split[0];
        public string[] GPRMCArray => GPRMC.Split(",");
        public float HDOP => float.Parse(split[1]);
        public double Altitude => double.Parse(split[2]);
        public string State => split.Get<string>(3);
        public string AD => split.Get<string>(4);
        public string BaseId => split.Get<string>(5);
        public string CSQ => split.Get<string>(6);
        public string Journey => split.Get<string>(7);
        
        public decimal Latitude => GpsUtil.ConvertDegreeAngleToDouble(@"(\d{2})(\d{2}).(\d{4})", GPRMCArray[2], GPRMCArray[3]);
        public decimal Longitude => GpsUtil.ConvertDegreeAngleToDouble(@"(\d{3})(\d{2}).(\d{4})", GPRMCArray[4], GPRMCArray[5]);
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
                GroupCollection time = new Regex(@"(\d{2})(\d{2})(\d{2}).(\d{2})").Matches(GPRMCArray[0])[0].Groups;
                GroupCollection date = new Regex(@"(\d{2})(\d{2})(\d{2})").Matches(GPRMCArray[8])[0].Groups;

                return DateTimeUtil.New(date[3].Value, date[2].Value, date[1].Value,
                    time[1].Value, time[2].Value, time[3].Value,
                    time[4].Value);
            }
        }
    }
}