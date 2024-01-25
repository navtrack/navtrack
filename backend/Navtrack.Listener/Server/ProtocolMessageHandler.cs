using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Navtrack.DataAccess.Model.Assets;
using Navtrack.DataAccess.Services.Assets;
using Navtrack.DataAccess.Services.Positions;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Models;
using Navtrack.Listener.Services;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Server;

[Service(typeof(IProtocolMessageHandler))]
public class ProtocolMessageHandler(
    ILogger<ProtocolMessageHandler> logger,
    IServiceProvider provider,
    IPositionService positionService,
    IAssetRepository assetRepository,
    IConnectionRepository connectionRepository) : IProtocolMessageHandler
{
    public async Task HandleMessage(ProtocolConnectionContext connectionContext, INetworkStreamWrapper networkStream,
        byte[] bytes)
    {
        ICustomMessageHandler customMessageHandler = GetMessageHandler(connectionContext.Protocol);

        MessageInput messageInput = new()
        {
            ConnectionContext = connectionContext,
            NetworkStream = networkStream,
            DataMessage = new DataMessage(bytes, connectionContext.Protocol.SplitMessageBy)
        };
        
        await connectionRepository.AddMessage(connectionContext.ConnectionId, messageInput.DataMessage.Bytes);

        logger.LogTrace("{ClientProtocol}: received {ConvertHexStringArrayToHexString}", connectionContext.Protocol,
            HexUtil.ConvertHexStringArrayToHexString(messageInput.DataMessage.Hex));

        try
        {
            List<Position>? locations = customMessageHandler.ParseRange(messageInput)?.ToList();

            if (locations != null && locations.Count != 0 && connectionContext.Device != null)
            {
                await PrepareContext(connectionContext, locations);

                SavePositionsResult result = await positionService.Save(connectionContext.Device,
                    connectionContext.MaxDate!.Value,
                    connectionContext.ConnectionId,
                    locations);

                SetPositionGroupId(result, connectionContext);
            }
        }
        catch (Exception e)
        {
            logger.LogCritical(e, "{Type}: Error parsing {DataMessageHex} ", customMessageHandler.GetType(),
                messageInput.DataMessage.Hex);
        }
    }

    private async Task PrepareContext(ProtocolConnectionContext context, IEnumerable<Position> locations)
    {
        if (context.Device?.AssetId == null && !string.IsNullOrEmpty(context.Device?.SerialNumber))
        {
            AssetDocument? asset = await assetRepository.Get(context.Device.SerialNumber, context.Protocol.Port);

            if (asset != null)
            {
                context.Device.AssetId = asset.Id;
                context.Device.DeviceId = asset.Device.Id;
            }
        }

        DateTime maxDate = locations.Max(x => x.Date);

        if (context.MaxDate == null || context.MaxDate < maxDate)
        {
            context.MaxDate = maxDate;
        }
    }

    private static void SetPositionGroupId(SavePositionsResult result, ProtocolConnectionContext connectionContext)
    {
        if (result.PositionGroupId != null && connectionContext.Device is { PositionGroupId: null })
        {
            connectionContext.Device.PositionGroupId = result.PositionGroupId;
        }
    }

    private ICustomMessageHandler GetMessageHandler(IProtocol protocol)
    {
        Type type = typeof(ICustomMessageHandler<>).MakeGenericType(protocol.GetType());

        IServiceScope serviceScope = provider.CreateScope();
        ICustomMessageHandler? customMessageHandler =
            (ICustomMessageHandler?)serviceScope.ServiceProvider.GetService(type);

        if (customMessageHandler == null)
        {
            throw new Exception($"There is no message handler implemented for {protocol}");
        }

        return customMessageHandler;
    }
}