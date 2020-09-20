using System;
using System.Collections.Generic;
using System.Linq;
using Navtrack.DataAccess.Model;

namespace Navtrack.Api.Services.Assets
{
    public class Trip
    {
        public Trip()
        {
            Locations = new List<LocationEntity>();
        }

        public List<LocationEntity> Locations { get; set; }
        public DateTime? Start => Locations.FirstOrDefault()?.DateTime;
        public DateTime? End => Locations.LastOrDefault()?.DateTime;

        public double Distance { get; set; }

        public int Id { get; set; }
    }
}