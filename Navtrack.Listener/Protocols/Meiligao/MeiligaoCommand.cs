using Navtrack.Listener.Helpers;

namespace Navtrack.Listener.Protocols.Meiligao
{
    public class MeiligaoCommand
    {
        private readonly MeiligaoCommands command;
        private readonly string[] HexDeviceId;
        private readonly string data;

        public MeiligaoCommand(MeiligaoCommands command, string[] hexDeviceId, string data)
        {
            this.command = command;
            this.HexDeviceId = hexDeviceId;
            this.data = data;
        }

        public string[] HexHeader = {"40", "40"};
        public string[] HexEnd = {"0D", "0A"};

        public string Reply =>
            $"{StringUtil.Join(HexHeader)}{StringUtil.Join(HexLength)}{StringUtil.Join(HexDeviceId)}{(int) command:X2}{data}";
        
        public string FullReply => $"{Reply}{Checksum}{StringUtil.Join(HexEnd)}";

        // 2 (start) + 2 (length) + 7 (device id) + 2 (command) + X (data) + 2 (checksum) + 2 (end)
        public int Length => 2 + 2 + 7 + 2 + data.Length / 2 + 2 + 2;

        public string[] HexLength => new[] {"00", $"{Length:X2}"};

        public string Checksum => ""; // MeiligaoUtil.ComputeChecksum(HexUtil.ConvertHexToHexArray(Reply));
    }
}