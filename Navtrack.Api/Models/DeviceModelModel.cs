namespace Navtrack.Api.Models
{
    public class DeviceModelModel
    {
        public int Id { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public int Port { get; set; }
        public string Protocol { get; set; }
        public string DisplayName { get; set; }
    }
}