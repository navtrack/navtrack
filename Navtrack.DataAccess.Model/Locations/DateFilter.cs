using System;

namespace Navtrack.DataAccess.Model.Locations;

public class DateFilter
{
    public virtual DateTime? StartDate { get; set; }
    public virtual DateTime? EndDate { get; set; }
}