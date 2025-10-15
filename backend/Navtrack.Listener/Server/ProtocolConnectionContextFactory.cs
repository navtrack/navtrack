using System;
using System.Threading.Tasks;
using Navtrack.Database.Model.Devices;
using Navtrack.Database.Services.Devices;
using Navtrack.Listener.Models;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Server;

[Service(typeof(IProtocolConnectionContextFactory))]
public class ProtocolConnectionContextFactory(IDeviceConnectionRepository deviceConnectionRepository)
    : IProtocolConnectionContextFactory
{
    public async Task<ProtocolConnectionContext> GetConnectionContext(IProtocol protocol, TcpClientAdapter tcpClient)
    {
        NetworkStreamWrapper networkStream = new(tcpClient);

        DeviceConnectionEntity deviceConnectionDocument = new()
        {
            Port = protocol.Port,
            IPAddress = networkStream.RemoteEndPoint,
            CreatedDate = DateTime.UtcNow
        };
        await deviceConnectionRepository.Add(deviceConnectionDocument);

        ProtocolConnectionContext connectionContext = new(networkStream, protocol, deviceConnectionDocument.Id);

        return connectionContext;
    }
}