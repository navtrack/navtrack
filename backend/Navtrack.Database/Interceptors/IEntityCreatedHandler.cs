using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Navtrack.Database.Interceptors;

/// <summary>
/// Implementations are invoked after a successful SaveChanges for every entity of type
/// <typeparamref name="TEntity"/> that was inserted in the just-completed save.
/// Handlers may add further entities to the supplied <see cref="DbContext"/>; the interceptor
/// will persist any pending changes in a follow-up SaveChanges call.
/// </summary>
public interface IEntityCreatedHandler<in TEntity> where TEntity : class
{
    Task Handle(TEntity entity, DbContext context, CancellationToken cancellationToken);
}
