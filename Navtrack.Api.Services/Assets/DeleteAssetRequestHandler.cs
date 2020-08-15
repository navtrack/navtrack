using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Navtrack.Api.Model.Assets.Requests;
using Navtrack.Api.Services.RequestHandlers;
using Navtrack.DataAccess.Model;
using Navtrack.DataAccess.Model.Custom;
using Navtrack.DataAccess.Repository;
using Navtrack.DataAccess.Services;
using Navtrack.Library.DI;

namespace Navtrack.Api.Services.Assets
{
    [Service(typeof(IRequestHandler<DeleteAssetRequest>))]
    public class DeleteAssetRequestHandler : BaseRequestHandler<DeleteAssetRequest>
    {
        private readonly IRepository repository;
        private readonly IAssetDataService assetDataService;

        public DeleteAssetRequestHandler(IRepository repository, IAssetDataService assetDataService)
        {
            this.repository = repository;
            this.assetDataService = assetDataService;
        }

        public override async Task Authorize(DeleteAssetRequest request)
        {
            if (!await assetDataService.UserHasRole(request.UserId, request.AssetId, UserAssetRole.Owner))
            {
                ApiResponse.IsUnauthorised();
            }
        }

        public override async Task Handle(DeleteAssetRequest request)
        {
            using IUnitOfWork unitOfWork = repository.CreateUnitOfWork();
            await unitOfWork.BeginTransaction();

            AssetEntity asset = await unitOfWork.GetEntities<AssetEntity>().FirstAsync(x => x.Id == request.AssetId);

            unitOfWork.Delete(asset);

            await unitOfWork.SaveChanges();
            await unitOfWork.CommitTransaction();
        }
    }
}