using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Navtrack.Library.DI;
using Navtrack.Listener.Server;

namespace Navtrack.Listener
{
    [Service(typeof(IListenerService))]
    public class ListenerService : IListenerService
    {
        private readonly IEnumerable<IProtocol> protocols;
        private readonly IProtocolHandler protocolHandler;

        public ListenerService(IEnumerable<IProtocol> protocols, IProtocolHandler protocolHandler)
        {
            this.protocols = protocols;
            this.protocolHandler = protocolHandler;
        }

        [SuppressMessage("ReSharper", "AssignmentIsFullyDiscarded")]
        public async Task Execute(CancellationToken cancellationToken)
        {
            foreach (IProtocol protocol in protocols)
            {
                _ = protocolHandler.HandleProtocol(cancellationToken, protocol, protocol.Port);

                foreach (int additionalPort in protocol.AdditionalPorts)
                {
                    _ = protocolHandler.HandleProtocol(cancellationToken, protocol, additionalPort);
                }
            }

            await Task.Delay(Timeout.Infinite, cancellationToken);
        }
    }
}