using System;
using System.Net;
using System.Threading.Tasks;
using Navtrack.DataAccess.Model;
using Navtrack.DataAccess.Repository;
using Navtrack.Library.DI;

namespace Navtrack.Common.Services
{
    [Service(typeof(IConnectionService))]
    public class ConnectionService : IConnectionService
    {
        private readonly IRepository repository;

        public ConnectionService(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task<Connection> NewConnection(IPEndPoint ipEndPoint)
        {
            using IUnitOfWork unitOfWork = repository.CreateUnitOfWork();
            
            Connection connection = new Connection
            {
                OpenedAt = DateTime.Now,
                RemoteEndPoint = ipEndPoint.ToString()
            };

            unitOfWork.Add(connection);

            await unitOfWork.SaveChanges();

            return connection;
        }

        public async Task MarkConnectionAsClosed(Connection connection)
        {
            using IUnitOfWork unitOfWork = repository.CreateUnitOfWork();

            connection.ClosedAt = DateTime.Now;

            unitOfWork.Update(connection);

            await unitOfWork.SaveChanges();
        }
    }
}