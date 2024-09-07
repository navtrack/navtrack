using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Navtrack.Api.Model.Common;

namespace Navtrack.Api.Model.Messages;

public class MessageListModel : ListModel<MessageModel>
{
    public float? AverageSpeed
    {
        get
        {
            double? average = Items.Where(x => x.Position.Speed > 0).Average(x => x.Position.Speed);

            return (float?)Math.Round(average.GetValueOrDefault());
        }
    }

    public float? AverageAltitude
    {
        get
        {
            double? average = Items.Average(x => x.Position.Altitude);

            return (float?)Math.Round(average.GetValueOrDefault());
        }
    }

    [Required]
    public long TotalCount { get; set; }
}