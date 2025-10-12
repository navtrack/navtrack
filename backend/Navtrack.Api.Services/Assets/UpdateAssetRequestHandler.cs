using System.Threading.Tasks;
using Navtrack.Api.Model.Errors;
using Navtrack.Api.Services.Common.Exceptions;
using Navtrack.Api.Services.Requests;
using Navtrack.Database.Model.Assets;
using Navtrack.Database.Services.Assets;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Api.Services.Assets;

[Service(typeof(IRequestHandler<UpdateAssetRequest>))]
public class UpdateAssetRequestHandler(IAssetRepository assetRepository) : BaseRequestHandler<UpdateAssetRequest>
{
    private AssetEntity? asset;

    public override async Task Validate(RequestValidationContext<UpdateAssetRequest> context)
    {
        asset = await assetRepository.GetById(context.Request.AssetId);
        asset.Return404IfNull();

        bool nameIsUsed =
            await assetRepository.NameIsUsed(asset.OrganizationId, context.Request.Model.Name, asset.Id);

        context.ValidationException.AddErrorIfTrue(
            nameIsUsed,
            nameof(context.Request.Model.Name),
            ApiErrorCodes.Asset_000002_NameAlreadyUsed);
    }

    public override Task Handle(UpdateAssetRequest request)
    {
        asset!.Name = request.Model.Name;
        
        return assetRepository.Update(asset);
    }
}