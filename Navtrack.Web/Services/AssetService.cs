using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Navtrack.DataAccess.Model;
using Navtrack.DataAccess.Repository;
using Navtrack.Library.DI;
using Navtrack.Library.Services;
using Navtrack.Web.Models;

namespace Navtrack.Web.Services
{
    [Service(typeof(IAssetService))]
    public class AssetService : IAssetService
    {
        private readonly IRepository repository;
        private readonly IMapper mapper;

        public AssetService(IRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<List<AssetModel>> GetAll()
        {
            List<Asset> objects =
                await repository.GetEntities<Asset>()
                    .Include(x => x.Device)
                    .ThenInclude(x => x.DeviceType)
                    .ToListAsync();

            List<AssetModel> mapped = objects.Select(mapper.Map<Asset, AssetModel>).ToList();

            return mapped;
        }

        public async Task<AssetModel> Get(int id)
        {
            Asset asset = await
                repository.GetEntities<Asset>()
                    .Include(x => x.Device)
                    .ThenInclude(x => x.DeviceType)
                    .FirstOrDefaultAsync(x => x.Id == id);

            return asset != null
                ? mapper.Map<Asset, AssetModel>(asset)
                : null;
        }

        public async Task ValidateModel(AssetModel asset, ModelStateDictionary modelState)
        {
            if (asset.DeviceId > 0 && await repository.GetEntities<Device>()
                    .AnyAsync(x => x.Id == asset.DeviceId && x.Asset != null))
            {
                modelState.AddModelError(nameof(AssetModel.DeviceId), "Device is already assigned to another asset.");
            }
        }

        public async Task Add(AssetModel asset)
        {
            using IUnitOfWork unitOfWork = repository.CreateUnitOfWork();

            Asset mapped = mapper.Map<AssetModel, Asset>(asset);

            unitOfWork.Add(mapped);

            await unitOfWork.SaveChanges();
        }
    }
}