using Microsoft.AspNetCore.Mvc;

namespace Navtrack.Api.Model.Devices
{
    public class DeviceConnectionRequestModel
    {
        public int Id { get; set; }
        
        [FromQuery]
        public int Page { get; set; } = 1;
        
        [FromQuery]
        public int PerPage { get; set; } = 20;
    }
}