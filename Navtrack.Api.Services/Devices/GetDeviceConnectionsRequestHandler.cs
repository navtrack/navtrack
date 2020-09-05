using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Navtrack.Api.Model;
using Navtrack.Api.Model.Devices;
using Navtrack.Api.Model.Devices.Requests;
using Navtrack.Api.Services.RequestHandlers;
using Navtrack.DataAccess.Model;
using Navtrack.DataAccess.Model.Custom;
using Navtrack.DataAccess.Repository;
using Navtrack.DataAccess.Services;
using Navtrack.Library.DI;

namespace Navtrack.Api.Services.Devices
{
    [Service(typeof(IRequestHandler<GetDeviceConnectionsRequest, TableResponse<DeviceConnectionResponseModel>>))]
    public class GetDeviceConnectionsRequestHandler : BaseRequestHandler<GetDeviceConnectionsRequest,
        TableResponse<DeviceConnectionResponseModel>>
    {
        private readonly IRepository repository;
        private readonly IAssetDataService assetDataService;

        public GetDeviceConnectionsRequestHandler(IRepository repository,
            IAssetDataService assetDataService)
        {
            this.repository = repository;
            this.assetDataService = assetDataService;
        }

        public override async Task Authorize(GetDeviceConnectionsRequest request)
        {
            if (!await assetDataService.UserHasRoleForDevice(request.UserId, UserAssetRole.Owner, request.DeviceId))
            {
                ApiResponse.IsUnauthorised();
            }
        }

        public override Task Validate(GetDeviceConnectionsRequest request)
        {
            if (request.Page < 1)
            {
                ApiResponse.AddError(nameof(DeviceConnectionRequestModel.Page), "The Page property must be >= 1.");
            }

            if (request.PerPage < 10 || request.PerPage > 100)
            {
                ApiResponse.AddError(nameof(DeviceConnectionRequestModel.Page),
                    "The PerPage property must be between 10 and 100.");
            }

            return Task.CompletedTask;
        }

        public override async Task<TableResponse<DeviceConnectionResponseModel>> Handle(
            GetDeviceConnectionsRequest request)
        {
            IQueryable<DeviceConnectionEntity> queryable = repository.GetEntities<DeviceConnectionEntity>()
                .Where(x => x.DeviceId == request.DeviceId);

            int totalResults = await queryable.CountAsync();

            return new TableResponse<DeviceConnectionResponseModel>
            {
                Results = await queryable.OrderByDescending(x => x.Id)
                    .Skip((request.Page - 1) * request.PerPage)
                    .Take(request.PerPage)
                    .Select(x => new DeviceConnectionResponseModel
                    {
                        Id = x.Id,
                        DeviceId = request.DeviceId,
                        OpenedAt = x.OpenedAt,
                        ClosedAt = x.ClosedAt,
                        RemoteEndPoint = x.RemoteEndPoint
                    })
                    .ToListAsync(),
                MaxPage = (totalResults + request.PerPage - 1) / request.PerPage,
                PerPage = request.PerPage,
                Page = request.Page,
                TotalResults = totalResults
            };
        }
    }
}