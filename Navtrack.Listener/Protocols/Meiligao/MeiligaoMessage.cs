using System;
using Navtrack.Listener.Extensions;
using Navtrack.Listener.Helpers;

namespace Navtrack.Listener.Protocols.Meiligao
{
    public class MeiligaoMessage
    {
        private readonly string message;
        private readonly string[] hexArray;
        private readonly byte[] byteArray;

        public MeiligaoMessage(string message)
        {
            this.message = message;
            byteArray = StringUtil.ConvertStringToByteArray(message);
            hexArray = HexUtil.ConvertByteArrayToHexArray(byteArray);
        }

        public bool HasValidChecksum => !string.IsNullOrEmpty(message) && Checksum == ComputedChecksum;
        public string DataMessage =>
            StringUtil.ConvertByteArrayToString(byteArray[new Range(DataStartIndex, DataEndIndex)]);

        private const int DataStartIndex = 13;
        private int DataEndIndex => ContainsEndingCharacters ? byteArray.Length - 5 : byteArray.Length - 3;
        private bool ContainsEndingCharacters => hexArray[^2] == "0D" && hexArray[^1] == "0A";
        private string Checksum =>
            ContainsEndingCharacters ? hexArray[^4] + hexArray[^3] : hexArray[^2] + hexArray[^1];
        private string ComputedChecksum
        {
            get
            {
                int endIndex = ContainsEndingCharacters ? hexArray.Length - 4 : hexArray.Length - 2;
                byte[] bytes = HexUtil.ConvertHexArrayToByteArray(hexArray[new Range(0, endIndex)]);
                ulong checksum = Crc16.Ccitt(bytes);

                string hexChecksum = checksum.ToString("X2");

                return hexChecksum;
            }
        }
    }
}