using System;
using System.Collections.Generic;
using System.Linq;

namespace Navtrack.Api.Model.Assets
{
    public class TripResponseModel
    {
        public TripResponseModel()
        {
            Locations = new List<LocationResponseModel>();
        }

        public int Number { get; set; }
        public DateTime? StartDate => Locations?.FirstOrDefault()?.DateTime;
        public DateTime? EndDate => Locations?.LastOrDefault()?.DateTime;
        public List<LocationResponseModel> Locations { get; set; }
        public int Distance { get; set; }
    }
}