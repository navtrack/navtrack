using System;

namespace Navtrack.Database.Model.Shared;

public class WorkScheduleDayEntity
{
    public DayOfWeek DayOfWeek { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
}