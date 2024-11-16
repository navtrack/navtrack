using System.Threading.Tasks;
using Navtrack.Api.Model.Messages;
using Navtrack.Api.Services.Common.Exceptions;
using Navtrack.Api.Services.DeviceMessages.Mappers;
using Navtrack.Api.Services.Requests;
using Navtrack.DataAccess.Model.Assets;
using Navtrack.DataAccess.Model.Devices.Messages.Filters;
using Navtrack.DataAccess.Services.Assets;
using Navtrack.DataAccess.Services.Devices;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Api.Services.DeviceMessages;

[Service(typeof(IRequestHandler<GetAssetMessagesRequest, MessageList>))]
public class GetAssetMessagesRequestHandler(
    IAssetRepository assetRepository,
    IDeviceMessageRepository deviceMessageRepository)
    : BaseRequestHandler<GetAssetMessagesRequest, MessageList>
{
    public override async Task Validate(RequestValidationContext<GetAssetMessagesRequest> context)
    {
        AssetDocument? asset = await assetRepository.GetById(context.Request.AssetId);
        asset.Return404IfNull();
    }

    public override async Task<MessageList> Handle(GetAssetMessagesRequest request)
    {
        GetMessagesResult messages = await deviceMessageRepository.GetMessages(new GetMessagesOptions
        {
            AssetId = request.AssetId,
            PositionFilter = request.Filter,
            Page = request.Page,
            Size = request.Size
        });

        MessageList result = MessageListMapper.Map(messages);

        return result;
    }
}