using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Navtrack.Api.Model.Organizations;
using Navtrack.Api.Services.Common.Exceptions;
using Navtrack.Api.Services.Requests;
using Navtrack.Database.Model.Organizations;
using Navtrack.Database.Model.Shared;
using Navtrack.Database.Services.Organizations;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Api.Services.Organizations;

[Service(typeof(IRequestHandler<UpdateOrganizationRequest>))]
public class UpdateOrganizationRequestHandler(
    IOrganizationRepository organizationRepository) : BaseRequestHandler<UpdateOrganizationRequest>
{
    private OrganizationEntity? organization;

    public override async Task Validate(RequestValidationContext<UpdateOrganizationRequest> context)
    {
        organization = await organizationRepository.GetById(context.Request.OrganizationId);
        organization.Return404IfNull();
    }

    public override async Task Handle(UpdateOrganizationRequest request)
    {
        if (!string.IsNullOrEmpty(request.Model.Name) && organization!.Name != request.Model.Name)
        {
            await organizationRepository.UpdateName(request.OrganizationId, request.Model.Name);
        }

        if (request.Model.WorkSchedules is { Count: > 0 })
        {
            WorkScheduleEntity workSchedule = organization!.WorkSchedule ?? new WorkScheduleEntity();

            foreach (WorkScheduleDayModel newSchedule in request.Model.WorkSchedules)
            {
                WorkScheduleDayEntity? existingSchedule = workSchedule.Days?
                    .FirstOrDefault(x => x.DayOfWeek == newSchedule.DayOfWeek);

                if (existingSchedule != null)
                {
                    existingSchedule.StartTime = newSchedule.StartTime;
                    existingSchedule.EndTime = newSchedule.EndTime;
                }
                else
                {
                    workSchedule.Days ??= [];

                    workSchedule.Days.Add(new WorkScheduleDayEntity
                    {
                        DayOfWeek = newSchedule.DayOfWeek,
                        StartTime = newSchedule.StartTime,
                        EndTime = newSchedule.EndTime
                    });
                }
            }

            await organizationRepository.UpdateWorkSchedules(request.OrganizationId, workSchedule);
        }
    }
}