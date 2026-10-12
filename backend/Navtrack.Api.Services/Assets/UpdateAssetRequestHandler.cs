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
    public override async Task Handle(UpdateAssetRequest request)
    {
        AssetEntity? asset = await assetRepository.GetById(request.AssetId);
        asset.Return404IfNull();

        bool nameIsUsed = await assetRepository.NameIsUsed(asset.OrganizationId, request.Model.Name, asset.Id);

        new ValidationApiException()
            .AddErrorIfTrue(nameIsUsed, nameof(request.Model.Name), ApiErrorCodes.Asset_NameAlreadyUsed)
            .ThrowIfInvalid();

        asset!.Name = request.Model.Name;
        
        await assetRepository.Update(asset);
    }
}
