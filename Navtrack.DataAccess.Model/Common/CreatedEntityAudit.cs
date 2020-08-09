using System;

namespace Navtrack.DataAccess.Model.Common
{
    public abstract class CreatedEntityAudit
    {
        public DateTime CreatedAt { get; set; }
    }
}