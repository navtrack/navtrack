using System;
using Navtrack.DataAccess.Model.Common;
using Navtrack.DataAccess.Repository;
using Navtrack.Library.DI;

namespace Navtrack.DataAccess.Interceptors
{
    [Service(typeof(IInterceptor))]
    public class AuditInterceptor : BaseInterceptor
    {
        public override void OnAdd<T>(T entity)
        {
            if (entity is CreatedEntityAudit createdEntityAudit)
            {
                createdEntityAudit.CreatedAt = DateTime.UtcNow;
            }
        }

        public override void OnUpdate<T>(T entity)
        {
            if (entity is EntityAudit entityAudit)
            {
                entityAudit.UpdatedAt = DateTime.UtcNow;
            }
        }
    }
}