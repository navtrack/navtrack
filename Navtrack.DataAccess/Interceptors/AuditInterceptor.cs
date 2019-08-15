using System;
using Navtrack.DataAccess.Model;
using Navtrack.DataAccess.Repository;
using Navtrack.Library.DI;

namespace Navtrack.DataAccess.Interceptors
{
    [Service(typeof(IInterceptor))]
    public class AuditInterceptor : BaseInterceptor
    {
        public override void OnAdd<T>(T entity)
        {
            EntityAudit entityAudit = entity as EntityAudit;

            if (entityAudit != null)
            {
                entityAudit.CreatedAt = DateTime.Now;
            }
        }
    }
}