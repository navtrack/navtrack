using System;
using System.Linq;
using Navtrack.Api.Model.Organizations;
using Navtrack.Database.Model.Shared;

namespace Navtrack.Api.Services.Organizations.Mappers;

public static class WorkScheduleModelMapper
{
    public static WorkScheduleModel Map(WorkScheduleEntity? source)
    {
        return new WorkScheduleModel
        {
            Days = Enum.GetValues<DayOfWeek>().Select(x =>
            {
                WorkScheduleDayEntity? workScheduleDayEntity = source?.Days.FirstOrDefault(d => d.DayOfWeek == x);

                return new WorkScheduleDayModel
                {
                    DayOfWeek = x,
                    StartTime = workScheduleDayEntity?.StartTime ?? new TimeOnly(9, 0),
                    EndTime = workScheduleDayEntity?.EndTime ?? new TimeOnly(17, 0)
                };
            }).ToList()
        };
    }
}