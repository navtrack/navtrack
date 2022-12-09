using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using Navtrack.Library.DI;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Models;
using Navtrack.Listener.Services;

namespace Navtrack.Listener.Server;

[Service(typeof(IMessageHandler))]
public class MessageHandler : IMessageHandler
{
    private readonly IServiceProvider serviceProvider;
    private readonly ILocationService locationService;
    private readonly ILogger<MessageHandler> logger;
    private readonly IConnectionService connectionService;

    public MessageHandler(IServiceProvider serviceProvider, ILocationService locationService,
        ILogger<MessageHandler> logger, IConnectionService connectionService)
    {
        this.serviceProvider = serviceProvider;
        this.locationService = locationService;
        this.logger = logger;
        this.connectionService = connectionService;
    }

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
                    
                await locationService.AddRange(locations, connectionMessageId);
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

        IServiceScope serviceScope = serviceProvider.CreateScope();
        ICustomMessageHandler customMessageHandler = (ICustomMessageHandler) serviceScope.ServiceProvider.GetService(type);

        if (customMessageHandler == null)
        {
            throw new Exception($"There is no message handler implemented for {protocol}");
        }

        return customMessageHandler;
    }
}