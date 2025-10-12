using System.Threading.Tasks;
using Navtrack.Api.Model.Messages;
using Navtrack.Api.Services.Common.Exceptions;
using Navtrack.Api.Services.DeviceMessages.Mappers;
using Navtrack.Api.Services.Requests;
using Navtrack.Database.Model.Assets;
using Navtrack.Database.Model.Filters;
using Navtrack.Database.Services.Assets;
using Navtrack.Database.Services.Devices;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Api.Services.DeviceMessages;

[Service(typeof(IRequestHandler<GetAssetMessagesRequest, MessageList>))]
public class GetAssetMessagesRequestHandler(
    IAssetRepository assetRepository,
    IDeviceMessageRepository deviceMessageRepository)
    : BaseRequestHandler<GetAssetMessagesRequest, MessageList>
{
    private AssetEntity? asset;

    public override async Task Validate(RequestValidationContext<GetAssetMessagesRequest> context)
    {
        asset = await assetRepository.GetById(context.Request.AssetId);
        asset.Return404IfNull();
    }

    public override async Task<MessageList> Handle(GetAssetMessagesRequest request)
    {
        GetMessagesResult messages = await deviceMessageRepository.GetMessages(new GetMessagesOptions
        {
            AssetId = asset.Id,
            PositionFilter = request.Filter,
            Page = request.Page,
            Size = request.Size
        });

        MessageList result = MessageListMapper.Map(messages);

        return result;
    }
}