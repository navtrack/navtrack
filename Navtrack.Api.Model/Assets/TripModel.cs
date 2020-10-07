using System;
using System.Collections.Generic;
using System.Linq;

namespace Navtrack.Api.Model.Assets
{
    public class TripModel
    {
        public TripModel()
        {
            Locations = new List<LocationModel>();
        }

        public int Number { get; set; }
        public DateTime? StartDate => Locations?.FirstOrDefault()?.DateTime;
        public DateTime? EndDate => Locations?.LastOrDefault()?.DateTime;
        public List<LocationModel> Locations { get; set; }
        public int Distance { get; set; }
    }
}