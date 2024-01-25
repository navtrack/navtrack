using System;

namespace Navtrack.DataAccess.Model.Positions;

public class DateFilter
{
    public virtual DateTime? StartDate { get; set; }
    public virtual DateTime? EndDate { get; set; }
}