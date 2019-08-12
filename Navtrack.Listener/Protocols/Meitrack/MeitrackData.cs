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
        public string MobileCountryCode { get; set; }
        public string MobileNetworkCode { get; set; }
        public string LocationAreaCode { get; set; }
        public string CellId { get; set; }
    }
}