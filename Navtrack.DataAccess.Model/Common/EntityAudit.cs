using System;

namespace Navtrack.DataAccess.Model.Common
{
    public abstract class EntityAudit : CreatedEntityAudit
    {
        public DateTime? UpdatedAt { get; set; }
    }
}