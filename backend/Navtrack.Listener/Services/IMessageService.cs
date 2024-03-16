using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using Navtrack.Listener.Models;

namespace Navtrack.Listener.Services;

public interface IMessageService
{
    Task Save(ObjectId connectionId, Device device, IEnumerable<Position> positions);
}