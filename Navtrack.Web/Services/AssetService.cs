using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Navtrack.DataAccess.Model;
using Navtrack.DataAccess.Repository;
using Navtrack.Library.DI;
using Navtrack.Library.Services;
using Navtrack.Web.Model.Models;

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

        public async Task<List<AssetModel>> Get()
        {
            List<Object> objects =
                await repository.GetEntities<Object>().ToListAsync();

            List<AssetModel> mapped = objects.Select(mapper.Map<Object, AssetModel>).ToList();

            return mapped;
        }
    }
}