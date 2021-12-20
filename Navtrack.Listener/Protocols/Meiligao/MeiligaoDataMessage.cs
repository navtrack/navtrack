using System;
using System.Linq;
using Navtrack.Listener.Helpers;

namespace Navtrack.Listener.Protocols.Meiligao;

public class MeiligaoDataMessage
{
    public readonly string[] StringSplit;
    public string GPRMC;
    public string[] GPRMCArray;
        
    public MeiligaoDataMessage(byte[] frame)
    {
        StringSplit = StringUtil.ConvertByteArrayToString(frame).Split("|");
        GPRMC = StringSplit[0];
        GPRMCArray = GPRMC.Split(",");
    }

    public string Checksum => GPRMCArray[^1].Replace("*", "");
    public string ChecksumComputed
    {
        get
        {
            string msg = GPRMC.Substring(0, GPRMC.IndexOf("*", StringComparison.InvariantCultureIgnoreCase)+1);

            int checksum = 0x100 - msg.Aggregate(0, (s, b) => s + b) & 0xFF & 0xFF;
             
            return checksum.ToString("X2");
        }
    }
    public bool ChecksumValid => Checksum == ChecksumComputed;
}