using System.Text.Json.Serialization;

namespace Navtrack.Listener.Protocols.Meitrack
{
    public class MeitrackData
    {
        public string GPSStatus { get; set; }
        public Event Event { get; set; }
        public int GSMSignal { get; set; }
        public string Data { get; set; }
        public int Journey { get; set; }
        public int Runtime { get; set; }
        public int[] Output { get; set; }
        public int[] Input { get; set; }
        
        [JsonPropertyName("MCC")]
        public int MobileCountryCode { get; set; }
        
        [JsonPropertyName("MNC")]
        public int MobileNetworkCode { get; set; }
        
        [JsonPropertyName("LAC")]
        public int LocationAreaCode { get; set; }
        
        public int CellId { get; set; }
        public long?[] Voltage { get; set; }
    }
}