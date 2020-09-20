using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Navtrack.Api.Model.Assets;
using Navtrack.Api.Services.RequestHandlers;
using Navtrack.DataAccess.Model;
using Navtrack.DataAccess.Model.Custom;
using Navtrack.DataAccess.Repository;
using Navtrack.DataAccess.Services;
using Navtrack.Library.DI;

namespace Navtrack.Api.Services.Assets
{
    [Service(typeof(ICommandHandler<RenameAssetCommand>))]
    public class RenameAssetCommandHandler : BaseCommandHandler<RenameAssetCommand>
    {
        private readonly IRepository repository;
        private readonly IAssetDataService assetDataService;

        public RenameAssetCommandHandler(IRepository repository, IAssetDataService assetDataService)
        {
            this.repository = repository;
            this.assetDataService = assetDataService;
        }

        public override async Task Authorize(RenameAssetCommand command)
        {
            if (!await assetDataService.UserHasRoleForAsset(command.UserId, UserAssetRole.Owner, command.AssetId))
            {
                ApiResponse.IsUnauthorised();
            }
        }

        public override async Task Validate(RenameAssetCommand command)
        {
            if (string.IsNullOrEmpty(command.Model.Name))
            {
                ApiResponse.AddError(nameof(AddAssetRequestModel.Name), "The name is required.");
            }
            else if (await repository.GetEntities<UserAssetEntity>().AnyAsync(x =>
                x.AssetId != command.AssetId &&
                x.UserId == command.UserId &&
                EF.Functions.Like(x.Asset.Name.ToLower(), $"{command.Model.Name.ToLower()}")))
            {
                ApiResponse.AddError(nameof(AddAssetRequestModel.Name),
                    "You already have an asset with the same name.");
            }
        }

        public override async Task Handle(RenameAssetCommand command)
        {
            IUnitOfWork unitOfWork = repository.CreateUnitOfWork();

            AssetEntity entity = await unitOfWork.GetEntities<AssetEntity>().FirstAsync(x => x.Id == command.AssetId);

            entity.Name = command.Model.Name;

            unitOfWork.Update(entity);

            await unitOfWork.SaveChanges();
        }
    }
}