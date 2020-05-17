using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Navtrack.Api.Models;
using Navtrack.Api.Services.Extensions;
using Navtrack.Api.Services.Generic;
using Navtrack.DataAccess.Model;
using Navtrack.DataAccess.Model.Common;
using Navtrack.DataAccess.Repository;
using Navtrack.Library.DI;
using Navtrack.Library.Services;

namespace Navtrack.Api.Services
{
    [Service(typeof(IAssetService))]
    [Service(typeof(IGenericService<AssetEntity, AssetModel>))]
    public class AssetService : GenericService<AssetEntity, AssetModel>, IAssetService
    {
        public AssetService(IRepository repository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
            : base(repository, mapper, httpContextAccessor)
        {
        }

        public override async Task<List<AssetModel>> GetAll()
        {
            var entities = await (from asset in GetQueryable()
                    join latestLocation in repository.GetEntities<LocationEntity>()
                            .GroupBy(x => x.AssetId)
                            .Select(x => new {AssetId = x.Key, LatestDateTime = x.Max(y => y.DateTime)})
                        on asset.Id equals latestLocation.AssetId into latestLocations
                    from latestLocation in latestLocations.DefaultIfEmpty()
                    join location in repository.GetEntities<LocationEntity>()
                        on new {AssetId = asset.Id, DateTime = latestLocation.LatestDateTime} equals new
                            {location.AssetId, location.DateTime}
                        into locations
                    from location in locations.DefaultIfEmpty()
                    select new {asset, location})
                .ToListAsync();

            List<AssetModel> models = entities.Select(x => mapper.Map<AssetEntity, LocationEntity, AssetModel>(x.asset, x.location))
                .ToList();

            return models;
        }

        protected override IQueryable<AssetEntity> GetQueryable()
        {
            return base.GetQueryable()
                .Include(x => x.Device)
                .Where(x => x.Users.Any(y => y.UserId == httpContextAccessor.HttpContext.User.GetId()));
        }

        protected override async Task ValidateSave(AssetModel asset, ValidationResult validationResult)
        {
            if (asset.DeviceId <= 0)
            {
                validationResult.AddError(nameof(AssetModel.DeviceId),
                    "A device is required, if none is available you must add one first.");
            }
            else if (await repository.GetEntities<DeviceEntity>()
                .AllAsync(x => x.Id != asset.DeviceId))
            {
                validationResult.AddError(nameof(AssetModel.DeviceId), "The device is not valid.");
            }
            else if (!await repository.GetEntities<DeviceEntity>()
                .AnyAsync(x =>
                    x.Id == asset.DeviceId &&
                    x.Users.Any(y => y.UserId == httpContextAccessor.HttpContext.User.GetId())))
            {
                validationResult.AddError(nameof(AssetModel.DeviceId),
                    "You do not have access to the selected device.");
            }
            else if (await repository.GetEntities<AssetEntity>()
                .AnyAsync(x => x.Id != asset.Id && x.DeviceId == asset.DeviceId))
            {
                validationResult.AddError(nameof(AssetModel.DeviceId),
                    "The device is already assigned to another asset.");
            }
        }

        protected override Task BeforeAdd(IUnitOfWork unitOfWork, AssetModel model, AssetEntity entity)
        {
            entity.Users.Add(new UserAssetEntity
            {
                UserId = httpContextAccessor.HttpContext.User.GetId(),
                RoleId = (int) EntityRole.Owner
            });

            return Task.CompletedTask;
        }

        protected override async Task<IUserRelation> GetUserRelation(int id)
        {
            UserAssetEntity userAsset = await repository.GetEntities<UserAssetEntity>()
                .FirstOrDefaultAsync(x => x.AssetId == id);

            return userAsset;
        }
    }
}