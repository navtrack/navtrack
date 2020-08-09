using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Navtrack.Api.Model.Assets;
using Navtrack.Api.Model.Assets.Requests;
using Navtrack.Api.Services.RequestHandlers;
using Navtrack.DataAccess.Model;
using Navtrack.DataAccess.Model.Custom;
using Navtrack.DataAccess.Repository;
using Navtrack.DataAccess.Services;
using Navtrack.Library.DI;

namespace Navtrack.Api.Services.Assets
{
    [Service(typeof(IRequestHandler<RenameAssetRequest>))]
    public class RenameAssetRequestHandler : BaseRequestHandler<RenameAssetRequest>
    {
        private readonly IRepository repository;
        private readonly IAssetDataService assetDataService;

        public RenameAssetRequestHandler(IRepository repository, IAssetDataService assetDataService)
        {
            this.repository = repository;
            this.assetDataService = assetDataService;
        }

        public override async Task Authorize(RenameAssetRequest request)
        {
            if (!await assetDataService.UserHasRole(request.UserId, request.AssetId, UserAssetRole.Owner))
            {
                ApiResponse.IsUnauthorised();
            }
        }

        public override async Task Validate(RenameAssetRequest request)
        {
            if (string.IsNullOrEmpty(request.Body.Name))
            {
                ApiResponse.AddError(nameof(AddAssetRequestModel.Name), "The name is required.");
            }
            else if (await repository.GetEntities<UserAssetEntity>().AnyAsync(x =>
                x.AssetId != request.AssetId &&
                x.UserId == request.UserId &&
                EF.Functions.Like(x.Asset.Name.ToLower(), $"{request.Body.Name.ToLower()}")))
            {
                ApiResponse.AddError(nameof(AddAssetRequestModel.Name),
                    "You already have an asset with the same name.");
            }
        }

        public override async Task Handle(RenameAssetRequest request)
        {
            IUnitOfWork unitOfWork = repository.CreateUnitOfWork();

            AssetEntity entity = await unitOfWork.GetEntities<AssetEntity>().FirstAsync(x => x.Id == request.AssetId);

            entity.Name = request.Body.Name;

            unitOfWork.Update(entity);

            await unitOfWork.SaveChanges();
        }
    }
}