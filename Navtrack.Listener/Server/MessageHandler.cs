using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Navtrack.Library.DI;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Models;
using Navtrack.Listener.Services;

namespace Navtrack.Listener.Server
{
    [Service(typeof(IMessageHandler))]
    public class MessageHandler : IMessageHandler
    {
        private readonly IServiceProvider serviceProvider;
        private readonly ILocationService locationService;
        private readonly ILogger<MessageHandler> logger;

        public MessageHandler(IServiceProvider serviceProvider, ILocationService locationService,
            ILogger<MessageHandler> logger)
        {
            this.serviceProvider = serviceProvider;
            this.locationService = locationService;
            this.logger = logger;
        }

        public async Task HandleMessage(Client client, INetworkStreamWrapper networkStream, byte[] bytes)
        {
            ICustomMessageHandler customMessageHandler = GetMessageHandler(client.Protocol);

            MessageInput messageInput = new MessageInput
            {
                Client = client,
                NetworkStream = networkStream,
                DataMessage = new DataMessage(bytes, client.Protocol.SplitMessageBy)
            };

            logger.LogTrace(
                $"{client.Protocol}: received {HexUtil.ConvertHexStringArrayToHexString(messageInput.DataMessage.Hex)}");

            try
            {
                List<Location> locations = customMessageHandler.ParseRange(messageInput)?.ToList();

                if (locations != null && locations.Any())
                {
                    await locationService.AddRange(locations);
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

            ICustomMessageHandler customMessageHandler = (ICustomMessageHandler) serviceProvider.GetService(type);

            if (customMessageHandler == null)
            {
                throw new Exception($"There is no message handler implemented for {protocol}");
            }

            return customMessageHandler;
        }
    }
}