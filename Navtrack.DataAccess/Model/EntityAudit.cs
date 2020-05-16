using System;

namespace Navtrack.DataAccess.Model
{
    public abstract class EntityAudit
    {
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}