using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Navtrack.Api.Model.Locations;
using Navtrack.Api.Model.Locations.Requests;
using Navtrack.Api.Services.RequestHandlers;
using Navtrack.DataAccess.Model;
using Navtrack.DataAccess.Repository;
using Navtrack.Library.DI;
using Navtrack.Library.Services;

namespace Navtrack.Api.Services.Locations
{
    [Service(typeof(IRequestHandler<GetLatestLocationRequest, LocationResponseModel>))]
    public class GetLatestLocationRequestHandler : BaseRequestHandler<GetLatestLocationRequest, LocationResponseModel>
    {
        private readonly IRepository repository;
        private readonly IMapper mapper;

        public GetLatestLocationRequestHandler(IRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public override async Task<LocationResponseModel> Handle(GetLatestLocationRequest request)
        {
            LocationEntity location = await repository.GetEntities<LocationEntity>()
                .Where(x => x.AssetId == request.AssetId && x.Asset.Users.Any(y => y.UserId == request.UserId))
                .OrderByDescending(x => x.DateTime)
                .FirstOrDefaultAsync();

            if (location != null)
            {
                LocationResponseModel mapped = mapper.Map<LocationEntity, LocationResponseModel>(location);

                return mapped;
            }

            return null;
        }
    }
}