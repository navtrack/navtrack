using System.Collections.Generic;

namespace Navtrack.Database.Model.Shared;

public class WorkScheduleEntity
{
    public List<WorkScheduleDayEntity> Days { get; set; }
}