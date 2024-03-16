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
    IMessageService messageService,
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
            List<Position>? positions = customMessageHandler.ParseRange(messageInput)?.ToList();

            if (positions is { Count: > 0 } && connectionContext.Device != null)
            {
                await PrepareContext(connectionContext);

                await messageService.Save(connectionContext.ConnectionId, connectionContext.Device, positions);
            }
        }
        catch (Exception e)
        {
            logger.LogCritical(e, "{Type}: Error parsing {DataMessageHex} ", customMessageHandler.GetType(),
                messageInput.DataMessage.Hex);
        }
    }

    private async Task PrepareContext(ProtocolConnectionContext context)
    {
        if (context.Device?.AssetId == null && !string.IsNullOrEmpty(context.Device?.SerialNumber))
        {
            AssetDocument? asset = await assetRepository.Get(context.Device.SerialNumber, context.Protocol.Port);

            if (asset is { Device: not null })
            {
                context.Device.DeviceId = asset.Device.Id;
                context.Device.AssetId = asset.Id;
                context.Device.MaxDate = asset.LastPositionMessage?.Position.Date;
            }
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