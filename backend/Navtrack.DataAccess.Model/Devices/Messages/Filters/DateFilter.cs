using System;

namespace Navtrack.DataAccess.Model.Devices.Messages.Filters;

public class DateFilter
{
    public virtual DateTime? StartDate { get; set; }
    public virtual DateTime? EndDate { get; set; }
}