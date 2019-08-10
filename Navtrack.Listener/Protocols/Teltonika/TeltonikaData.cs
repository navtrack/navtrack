using System.Collections.Generic;

namespace Navtrack.Listener.Protocols.Teltonika
{
    public class TeltonikaData
    {
        public List<Event> Events { get; set; }
//        public int Priority { get; set; }
//        public int EventId { get; set; }
//        public string DigitalInputs { get; set; }
        public bool Movement { get; set; }
        public int GsmSignal { get; set; }
        public string CurrentProfile { get; set; }
        public string MobileCountryCode { get; set; }
        public string MobileNetworkCode { get; set; }
        public double Voltage { get; set; }
        public int Odometer { get; set; }
        public byte[] Input { get; set; }
    }
}