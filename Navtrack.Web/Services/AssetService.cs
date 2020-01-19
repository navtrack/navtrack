using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Navtrack.DataAccess.Model;
using Navtrack.DataAccess.Repository;
using Navtrack.Library.DI;
using Navtrack.Library.Services;
using Navtrack.Web.Models;
using Navtrack.Web.Services.Generic;

namespace Navtrack.Web.Services
{
    [Service(typeof(IAssetService))]
    [Service(typeof(IGenericService<Asset, AssetModel>))]
    public class AssetService : GenericService<Asset, AssetModel>, IAssetService
    {
        private readonly IRepository repository;
        private readonly IMapper mapper;

        public AssetService(IRepository repository, IMapper mapper) : base(repository, mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        protected override IQueryable<Asset> GetQueryable()
        { 
            return base.GetQueryable()
                .Include(x => x.Device)
                .ThenInclude(x => x.DeviceType);
        }

        protected override async Task ValidateSave(AssetModel asset, ValidationResult validationResult)
        {
            if (asset.DeviceId > 0 && await repository.GetEntities<Device>()
                    .AnyAsync(x => x.Id == asset.DeviceId && x.Asset != null))
            {
                validationResult.AddError(nameof(AssetModel.DeviceId), "Device is already assigned to another asset.");
            }
        }

        public async Task<AssetStatsModel> GetStats()
        {
            AssetStatsModel model = new AssetStatsModel
            {
                OnlineAssets = await repository.GetEntities<Asset>()
                    .Where(x => x.Locations.Any(y => y.DateTime > DateTime.Now.AddMinutes(-5))).CountAsync(),
                TotalAssets = await repository.GetEntities<Asset>().CountAsync()
            };

            return model;
        }
    }
}