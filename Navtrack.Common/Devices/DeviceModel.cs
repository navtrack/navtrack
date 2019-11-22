namespace Navtrack.Common.Devices
{
    public class DeviceModel
    {
        public string Brand { get; set; }
        public string Model { get; set; }
        public Protocol Protocol { get; set; }
        
        public DeviceModel(string brand, string model, Protocol protocol)
        {
            Brand = brand;
            Model = model;
            Protocol = protocol;
        }
    }
}