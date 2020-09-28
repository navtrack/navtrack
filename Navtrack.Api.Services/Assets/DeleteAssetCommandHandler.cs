using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Navtrack.Api.Model.Assets;
using Navtrack.Api.Services.CommandHandler;
using Navtrack.DataAccess.Model;
using Navtrack.DataAccess.Model.Custom;
using Navtrack.DataAccess.Repository;
using Navtrack.DataAccess.Services;
using Navtrack.Library.DI;

namespace Navtrack.Api.Services.Assets
{
    [Service(typeof(ICommandHandler<DeleteAssetCommand>))]
    public class DeleteAssetCommandHandler : BaseCommandHandler<DeleteAssetCommand>
    {
        private readonly IRepository repository;
        private readonly IAssetDataService assetDataService;

        public DeleteAssetCommandHandler(IRepository repository, IAssetDataService assetDataService)
        {
            this.repository = repository;
            this.assetDataService = assetDataService;
        }

        public override async Task Authorize(DeleteAssetCommand command)
        {
            if (!await assetDataService.UserHasRoleForAsset(command.UserId, UserAssetRole.Owner, command.AssetId))
            {
                ApiResponse.IsUnauthorised();
            }
        }

        public override async Task Handle(DeleteAssetCommand command)
        {
            using IUnitOfWork unitOfWork = repository.CreateUnitOfWork();
            await unitOfWork.BeginTransaction();

            AssetEntity asset = await unitOfWork.GetEntities<AssetEntity>().FirstAsync(x => x.Id == command.AssetId);

            unitOfWork.Delete(asset);

            await unitOfWork.SaveChanges();
            await unitOfWork.CommitTransaction();
        }
    }
}