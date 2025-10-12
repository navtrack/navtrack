using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Navtrack.Database.Model.Assets;
using Navtrack.Database.Model.Devices;
using Navtrack.Database.Services.Assets;
using Navtrack.Database.Services.Devices;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Models;
using Navtrack.Listener.Services;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Server;

[Service(typeof(IProtocolMessageHandler))]
public class ProtocolMessageHandler(
    ILogger<ProtocolMessageHandler> logger,
    IServiceProvider provider,
    IDeviceMessageService deviceMessageService,
    IAssetRepository assetRepository,
    IDeviceConnectionRepository deviceConnectionRepository) : IProtocolMessageHandler
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

        if (MessageIsBlacklisted(messageInput))
        {
            connectionContext.NetworkStream.Close();
            return;
        }

        await deviceConnectionRepository.AddMessage(connectionContext.ConnectionId, messageInput.DataMessage.Bytes);

        logger.LogTrace("{ClientProtocol}: received {ConvertHexStringArrayToHexString}", connectionContext.Protocol,
            HexUtil.ConvertHexStringArrayToHexString(messageInput.DataMessage.Hex));

        List<DeviceMessageEntity>? messages = customMessageHandler.ParseRange(messageInput)?.ToList();

        if (messages is { Count: > 0 } && connectionContext.Device != null)
        {
            await PrepareContext(connectionContext);

            SaveDeviceMessageResult? result = await deviceMessageService.Save(new SaveDeviceMessageInput
            {
                Device = connectionContext.Device,
                ConnectionId = connectionContext.ConnectionId,
                Messages = messages
            });
            
            HandleResult(connectionContext, result);
        }
    }

    private static void HandleResult(ProtocolConnectionContext connectionContext, SaveDeviceMessageResult? result)
    {
        if (result?.MaxPositionDate != null && connectionContext.Device != null)
        {
            connectionContext.Device.MaxDate = result.MaxPositionDate;
        }
    }

    private static bool MessageIsBlacklisted(MessageInput messageInput)
    {
        string[] blacklistedMessages =
        [
            "Host:",
            "MGLNDD"
        ];

        return blacklistedMessages.Any(x =>
            messageInput.DataMessage.String.Contains(x, StringComparison.InvariantCultureIgnoreCase));
    }

    private async Task PrepareContext(ProtocolConnectionContext context)
    {
        if (context.Device?.AssetId == null && !string.IsNullOrEmpty(context.Device?.SerialNumber))
        {
            AssetEntity? asset = await assetRepository.Get(context.Device.SerialNumber, context.Protocol.Port);

            if (asset is { Device: not null })
            {
                context.Device.DeviceId = asset.Device.Id;
                context.Device.AssetId = asset.Id;
                context.Device.MaxDate = asset.LastPositionMessage?.Date;
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