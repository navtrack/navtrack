using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Navtrack.Api.Model.Custom;
using Navtrack.Api.Model.Devices;
using Navtrack.Api.Services.RequestHandlers;
using Navtrack.DataAccess.Model;
using Navtrack.DataAccess.Model.Custom;
using Navtrack.DataAccess.Repository;
using Navtrack.DataAccess.Services;
using Navtrack.Library.DI;

namespace Navtrack.Api.Services.Devices
{
    [Service(
        typeof(IRequestHandler<GetDeviceConnectionsCommand, ResultsPaginationModel<DeviceConnectionModel>>))]
    public class GetDeviceConnectionsCommandHandler : BaseRequestHandler<GetDeviceConnectionsCommand,
        ResultsPaginationModel<DeviceConnectionModel>>
    {
        private readonly IRepository repository;
        private readonly IAssetDataService assetDataService;

        public GetDeviceConnectionsCommandHandler(IRepository repository,
            IAssetDataService assetDataService)
        {
            this.repository = repository;
            this.assetDataService = assetDataService;
        }

        public override async Task Authorize(GetDeviceConnectionsCommand command)
        {
            if (!await assetDataService.UserHasRoleForDevice(command.UserId, UserAssetRole.Owner, command.DeviceId))
            {
                ApiResponse.IsUnauthorised();
            }
        }

        public override Task Validate(GetDeviceConnectionsCommand command)
        {
            if (command.Page < 1)
            {
                ApiResponse.AddError(nameof(DeviceConnectionRequestModel.Page), "The Page property must be >= 1.");
            }

            if (command.PerPage < 10 || command.PerPage > 100)
            {
                ApiResponse.AddError(nameof(DeviceConnectionRequestModel.PerPage),
                    "The PerPage property must be between 10 and 100.");
            }

            return Task.CompletedTask;
        }

        public override async Task<ResultsPaginationModel<DeviceConnectionModel>> Handle(
            GetDeviceConnectionsCommand command)
        {
            IQueryable<DeviceConnectionEntity> queryable = repository.GetEntities<DeviceConnectionEntity>()
                .Where(x => x.DeviceId == command.DeviceId);

            int totalResults = await queryable.CountAsync();

            List<DeviceConnectionEntity> results = await queryable.OrderByDescending(x => x.Id)
                .Skip((command.Page - 1) * command.PerPage)
                .Take(command.PerPage)
                .ToListAsync();

            List<int> deviceConnectionsIds = results.Select(x => x.Id).ToList();

            var messageCounts =
                await repository.GetEntities<DeviceConnectionMessageEntity>()
                    .Where(x => deviceConnectionsIds.Contains(x.DeviceConnectionId))
                    .GroupBy(x => x.DeviceConnectionId)
                    .Select(x => new {DeviceConnectionId = x.Key, Messages = x.Count()})
                    .ToListAsync();

            return new ResultsPaginationModel<DeviceConnectionModel>
            {
                Results = results
                    .Select(x => new DeviceConnectionModel
                    {
                        Id = x.Id,
                        DeviceId = command.DeviceId,
                        OpenedAt = x.OpenedAt,
                        ClosedAt = x.ClosedAt,
                        RemoteEndPoint = x.RemoteEndPoint,
                        Messages = (messageCounts.FirstOrDefault(y => y.DeviceConnectionId == x.Id)?.Messages)
                            .GetValueOrDefault()
                    }).ToList(),
                Pagination =
                {
                    MaxPage = (totalResults + command.PerPage - 1) / command.PerPage,
                    PerPage = command.PerPage,
                    Page = command.Page,
                    TotalResults = totalResults
                }
            };
        }
    }
}