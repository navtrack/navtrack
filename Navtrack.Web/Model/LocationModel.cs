using System;

namespace Navtrack.Web.Model
{
    public class LocationModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public DateTime DateTime { get; set; }
    }
}