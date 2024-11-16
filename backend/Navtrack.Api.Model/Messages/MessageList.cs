using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Navtrack.Api.Model.Common;

namespace Navtrack.Api.Model.Messages;

public class MessageList : List<Message>
{
    public float? AverageSpeed
    {
        get
        {
            System.Collections.Generic.List<Message> filtered = Items.Where(x => x.Position.Speed > 0).ToList();
            
            double? average = filtered.Count != 0 ? filtered.Average(x => x.Position.Speed) : null;

            return (float?)Math.Round(average.GetValueOrDefault());
        }
    }

    public float? AverageAltitude
    {
        get
        {
            double? average = Items.Any() ? Items.Average(x => x.Position.Altitude) : null;

            return (float?)Math.Round(average.GetValueOrDefault());
        }
    }

    [Required]
    public long TotalCount { get; set; }
}