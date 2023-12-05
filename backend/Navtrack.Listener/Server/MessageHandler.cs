using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Models;
using Navtrack.Listener.Services;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Server;

[Service(typeof(IMessageHandler))]
public class MessageHandler(
    IServiceProvider provider,
    ILocationService service,
    ILogger<MessageHandler> logger,
    IConnectionService connectionService)
    : IMessageHandler
{
    public async Task HandleMessage(Client client, INetworkStreamWrapper networkStream, byte[] bytes)
    {
        ICustomMessageHandler customMessageHandler = GetMessageHandler(client.Protocol);

        MessageInput messageInput = new()
        {
            Client = client,
            NetworkStream = networkStream,
            DataMessage = new DataMessage(bytes, client.Protocol.SplitMessageBy)
        };

        logger.LogTrace(
            $"{client.Protocol}: received {HexUtil.ConvertHexStringArrayToHexString(messageInput.DataMessage.Hex)}");

        ObjectId connectionMessageId = await connectionService.AddMessage(client.DeviceConnection.Id, messageInput.DataMessage.HexString);

        try
        {
            List<Location> locations = customMessageHandler.ParseRange(messageInput)?.ToList();

            if (locations != null && locations.Any())
            {
                // TODO refactor this
                await connectionService.SetDeviceId(client);
                    
                await service.AddRange(locations, connectionMessageId);
            }
        }
        catch (Exception e)
        {
            logger.LogCritical(e,
                $"{customMessageHandler.GetType()}: Error parsing {messageInput.DataMessage.Hex} ");
        }
    }

    private ICustomMessageHandler GetMessageHandler(IProtocol protocol)
    {
        Type type = typeof(ICustomMessageHandler<>).MakeGenericType(protocol.GetType());

        IServiceScope serviceScope = provider.CreateScope();
        ICustomMessageHandler customMessageHandler = (ICustomMessageHandler) serviceScope.ServiceProvider.GetService(type);

        if (customMessageHandler == null)
        {
            throw new Exception($"There is no message handler implemented for {protocol}");
        }

        return customMessageHandler;
    }
}