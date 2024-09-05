using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Navtrack.Api.Model.Common;

namespace Navtrack.Api.Model.Positions;

public class PositionListModel : ListModel<PositionModel>
{
    public float? AverageSpeed
    {
        get
        {
            double? average = Items.Where(x => x.Speed > 0).Average(x => x.Speed);

            return (float?)Math.Round(average.GetValueOrDefault());
        }
    }

    public float? AverageAltitude
    {
        get
        {
            double? average = Items.Average(x => x.Altitude);

            return (float?)Math.Round(average.GetValueOrDefault());
        }
    }

    [Required]
    public long TotalCount { get; set; }
}