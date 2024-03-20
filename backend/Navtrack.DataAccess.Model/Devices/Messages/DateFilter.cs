using System;

namespace Navtrack.DataAccess.Model.Devices.Messages;

public class DateFilter
{
    public virtual DateTime? StartDate { get; set; }
    public virtual DateTime? EndDate { get; set; }
}