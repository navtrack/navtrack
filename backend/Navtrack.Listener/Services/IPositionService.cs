using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using Navtrack.Listener.Models;

namespace Navtrack.Listener.Services;

public interface IPositionService
{
    Task<SavePositionsResult> Save(Device device, DateTime maxEndDate, ObjectId connectionId,
        IEnumerable<Position> locations);
}