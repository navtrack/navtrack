using System;
using MongoDB.Bson;

namespace Navtrack.Listener.Services;

public class SavePositionsResult
{
    public ObjectId? PositionGroupId { get; set; }
    public DateTime MaxDate { get; set; }
    public bool Success { get; set; }
}