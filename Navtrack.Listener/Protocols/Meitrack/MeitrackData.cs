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
        public bool[] Output { get; set; }
        public bool[] Input { get; set; }
        
        [JsonPropertyName("MCC")]
        public string MobileCountryCode { get; set; }
        
        [JsonPropertyName("MNC")]
        public string MobileNetworkCode { get; set; }
        
        [JsonPropertyName("LAC")]
        public int LocationAreaCode { get; set; }
        
        public int CellId { get; set; }
    }
}