using System.Collections.Generic;

namespace Navtrack.Api.Model.Organizations;

public class WorkScheduleModel
{
    public List<WorkScheduleDayModel> Days { get; set; }
}