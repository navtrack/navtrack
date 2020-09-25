using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Navtrack.DataAccess.Model;
using Navtrack.DataAccess.Repository;
using Navtrack.Library.DI;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Services
{
    [Service(typeof(IConnectionService))]
    public class ConnectionService : IConnectionService
    {
        private readonly IRepository repository;

        public ConnectionService(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task<DeviceConnectionEntity> NewConnection(string endPoint, int protocolPort)
        {
            using IUnitOfWork unitOfWork = repository.CreateUnitOfWork();

            DeviceConnectionEntity deviceConnection = new DeviceConnectionEntity
            {
                OpenedAt = DateTime.UtcNow,
                RemoteEndPoint = endPoint,
                ProtocolPort = protocolPort
            };

            unitOfWork.Add(deviceConnection);

            await unitOfWork.SaveChanges();

            return deviceConnection;
        }

        public async Task MarkConnectionAsClosed(DeviceConnectionEntity deviceConnection)
        {
            using IUnitOfWork unitOfWork = repository.CreateUnitOfWork();

            deviceConnection.ClosedAt = DateTime.UtcNow;

            unitOfWork.Update(deviceConnection);

            await unitOfWork.SaveChanges();
        }

        public async Task<int> AddMessage(int deviceConnectionId, string hex)
        {
            using IUnitOfWork unitOfWork = repository.CreateUnitOfWork();

            DeviceConnectionMessageEntity entity = new DeviceConnectionMessageEntity
            {
                DeviceConnectionId = deviceConnectionId,
                Hex = hex
            };

            unitOfWork.Add(entity);

            await unitOfWork.SaveChanges();

            return entity.Id;
        }

        public async Task SetDeviceId(Client client)
        {
            // TODO refactor this
            if (client.Device != null && client.Device.Entity == null && !string.IsNullOrEmpty(client.Device.IMEI))
            {
                client.Device.Entity = await repository.GetEntities<DeviceEntity>()
                    .FirstOrDefaultAsync(
                        x => x.DeviceId == client.Device.IMEI && x.ProtocolPort == client.Protocol.Port);

                if (client.Device.Entity != null)
                {
                    using IUnitOfWork unitOfWork = repository.CreateUnitOfWork();

                    client.DeviceConnection.DeviceId = client.Device.Entity.Id;

                    unitOfWork.Update(client.DeviceConnection);

                    await unitOfWork.SaveChanges();
                }
            }
        }
    }
}