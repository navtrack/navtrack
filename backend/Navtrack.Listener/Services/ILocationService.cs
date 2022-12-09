using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using Navtrack.Listener.Models;

namespace Navtrack.Listener.Services;

public interface ILocationService
{
    Task AddRange(List<Location> locations, ObjectId connectionMessageId);
}