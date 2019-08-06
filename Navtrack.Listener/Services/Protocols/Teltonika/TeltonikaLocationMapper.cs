//using System.Collections.Generic;
//using System.Globalization;
//using System.Linq;
//using System.Text.Json;
//using Navtrack.Common.Model;
//using Navtrack.Library.DI;
//using Navtrack.Library.Services;
//
//namespace Navtrack.Listener.Services.Protocols.Teltonika
//{
//    [Service(typeof(IMapper<Location, TeltonikaLocation>))]
//    public class TeltonikaLocationMapper : IMapper<Location, TeltonikaLocation>
//    {
//        public TeltonikaLocation Map(Location source, TeltonikaLocation destination)
//        {
//            TeltonikaLocation teltonikaLocation = (TeltonikaLocation)source;
//            teltonikaLocation.Events = new List<Event>();
//
//
//            // TIME
//            string tmp = Utility.GetNextBytes(input, ref position, 8);
//            destination.DateTime = UnixTimeStampToDateTime(long.Parse(tmp, NumberStyles.HexNumber) / 1000);
//
//            // PRIORITY
//            destination.Priority = input[position++];
//
//            // LONGITUDE
//            tmp = Utility.GetNextBytes(input, ref position, 4);
//            destination.Longitude = GetCoordinate(tmp);
//
//            // LATITUDE
//            tmp = Utility.GetNextBytes(input, ref position, 4);
//            destination.Latitude = GetCoordinate(tmp);
//
//            // ALTITUDE
//            tmp = Utility.GetNextBytes(input, ref position, 2);
//            destination.Altitude = int.Parse(tmp, NumberStyles.HexNumber);
//
//            // ANGLE
//            tmp = Utility.GetNextBytes(input, ref position, 2);
//            destination.Heading = int.Parse(tmp, NumberStyles.HexNumber);
//
//            // SATELLITES
//            tmp = Utility.ConvertToHex(input[position++]);
//            location.Satellites = short.Parse(tmp, NumberStyles.HexNumber);
//
//            // SPEED
//            tmp = Utility.GetNextBytes(input, ref position, 2);
//            destination.Speed = int.Parse(tmp, NumberStyles.HexNumber);
//
//            // EVENT ID
//            tmp = Utility.ConvertToHex(input[position++]);
//            destination.EventId = int.Parse(tmp, NumberStyles.HexNumber);
//
//            // NUMBER of events
//            position++; // skip over the number of events
//
//            // 1 BYTE VALUE EVENTS
//            destination.Events.AddRange(GetEvents(input, ref position, 1));
//
//            // 2 BYTES VALUE EVENTS
//            destination.Events.AddRange(GetEvents(input, ref position, 2));
//
//            // 4 BYTES VALUE EVENTS
//            destination.Events.AddRange(GetEvents(input, ref position, 4));
//
//            // 8 BYTES VALUE EVENTS
//            destination.Events.AddRange(GetEvents(input, ref position, 8));
//
//            destination.Movement = GetMovement(location.Events);
////            destination.Voltage = GetVoltage(location.Events);
////            destination.Odometer = GetOdometer(location.Events);
//            destination.CurrentProfile = GetCurrentProfile(location.Events);
//            destination.GsmSignal = GetGsmSignal(location.Events);
//            int operatorCode = GetOperatorCode(location.Events);
//            destination.MobileCountryCode = (operatorCode / 100).ToString(CultureInfo.InvariantCulture);
//            destination.MobileNetworkCode = (operatorCode % 100).ToString(CultureInfo.InvariantCulture);
//            destination.DigitalInputs = GetDigitalInput(location.Events);
//
//            destination.ProtocolData = JsonSerializer.Serialize(location.Events); ;
//
//            locations.Add(location);
//        }
//
//
//        private static int GetOperatorCode(IEnumerable<Event> events)
//        {
//            Event operatorCodeEvent = events.FirstOrDefault(x => x.Id == 241);
//
//            if (operatorCodeEvent != null)
//            {
//                return operatorCodeEvent.Value;
//            }
//
//            return 0;
//        }
//
//        private static int GetGsmSignal(IEnumerable<Event> events)
//        {
//            Event gsmSignalEvent = events.FirstOrDefault(x => x.Id == 21);
//
//            if (gsmSignalEvent != null)
//            {
//                return gsmSignalEvent.Value * 20;
//            }
//
//            return 0;
//        }
//
//        private static string GetCurrentProfile(IEnumerable<Event> events)
//        {
//            Event currentProfileEvent = events.FirstOrDefault(x => x.Id == 22);
//
//            return currentProfileEvent != null ? currentProfileEvent.Value.ToString(CultureInfo.InvariantCulture) : string.Empty;
//        }
//
//        private static bool GetMovement(IEnumerable<Event> events)
//        {
//            Event movementEvent = events.FirstOrDefault(x => x.Id == 240);
//
//            if (movementEvent?.Value == 1) return true;
//
//            return false;
//        }
//
//        private static double GetVoltage(IEnumerable<Event> events)
//        {
//            Event voltageEvent = events.FirstOrDefault(x => x.Id == 66);
//
//            if (voltageEvent != null)
//            {
//                return (double)voltageEvent.Value / 1000;
//            }
//
//            return 0;
//        }
//        private static int GetOdometer(IEnumerable<Event> events)
//        {
//            Event odometerEvent = events.FirstOrDefault(x => x.Id == 199);
//
//            return odometerEvent?.Value ?? 0;
//        }
//    }
//}