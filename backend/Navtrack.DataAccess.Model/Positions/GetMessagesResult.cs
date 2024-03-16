using System.Collections.Generic;

namespace Navtrack.DataAccess.Model.Positions;

public class GetMessagesResult
{
    public required long TotalCount { get; init; }
    public required List<MessageDocument> Messages { get; init; }
}