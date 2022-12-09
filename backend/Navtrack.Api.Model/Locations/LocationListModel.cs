using System;
using System.Linq;
using Navtrack.Api.Model.Common;

namespace Navtrack.Api.Model.Locations;

public class LocationListModel : ListModel<LocationModel>
{
    public float? AverageSpeed
    {
        get
        {
            float? average = Items.Where(x => x.Speed > 0).Average(x => x.Speed);

            return (float?)Math.Round(average.GetValueOrDefault());
        }
    }

    public float? AverageAltitude
    {
        get
        {
            float? average = Items.Average(x => x.Altitude);

            return (float?)Math.Round(average.GetValueOrDefault());
        }
    }
}