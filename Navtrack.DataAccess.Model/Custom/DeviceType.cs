namespace Navtrack.DataAccess.Model.Custom
{
    public class DeviceType
    {
        public int Id { get; set; }
        public string Manufacturer { get; set; }
        public string Type { get; set; }
        public Protocol Protocol { get; set; }
    }
}