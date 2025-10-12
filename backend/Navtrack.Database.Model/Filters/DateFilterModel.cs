using System;

namespace Navtrack.Database.Model.Filters;

public class DateFilterModel
{
    public virtual DateTime? StartDate { get; set; }
    public virtual DateTime? EndDate { get; set; }
}