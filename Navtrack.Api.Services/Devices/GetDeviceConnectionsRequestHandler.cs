using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Navtrack.Api.Model.Devices;
using Navtrack.Api.Model.Devices.Requests;
using Navtrack.Api.Services.RequestHandlers;
using Navtrack.DataAccess.Model;
using Navtrack.DataAccess.Model.Custom;
using Navtrack.DataAccess.Repository;
using Navtrack.Library.DI;
using Navtrack.Library.Services;

namespace Navtrack.Api.Services.Devices
{
    [Service(typeof(IRequestHandler<GetDeviceConnectionsRequest, IEnumerable<DeviceConnectionResponseModel>>))]
    public class GetDeviceConnectionsRequestHandler : BaseRequestHandler<GetDeviceConnectionsRequest,
        IEnumerable<DeviceConnectionResponseModel>>
    {
        private readonly IMapper mapper;
        private readonly IRepository repository;

        public GetDeviceConnectionsRequestHandler(IMapper mapper, IRepository repository)
        {
            this.mapper = mapper;
            this.repository = repository;
        }

        public override async Task<IEnumerable<DeviceConnectionResponseModel>> Handle(
            GetDeviceConnectionsRequest request)
        {
            return await repository.GetEntities<DeviceConnectionEntity>()
                .Where(x => x.Id == request.DeviceId && x.Device.Asset.Users.Any(y =>
                    y.UserId == request.UserId && y.RoleId == (int) UserAssetRole.Owner))
                .Select(x => new DeviceConnectionResponseModel
                {
                    Id = x.Id,
                    DeviceId = request.DeviceId,
                    OpenedAt = x.OpenedAt,
                    ClosedAt = x.ClosedAt,
                    RemoteEndPoint = x.RemoteEndPoint
                }).ToListAsync();
        }
    }
}