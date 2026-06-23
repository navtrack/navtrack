using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Navtrack.Database.Interceptors;

/// <summary>
/// Captures entities transitioning to the Added or Deleted state during SaveChanges and, after
/// the save succeeds, dispatches them to all registered
/// <see cref="IEntityCreatedHandler{TEntity}"/> or <see cref="IEntityDeletedHandler{TEntity}"/>
/// implementations. Handlers may stage additional changes on the same <see cref="DbContext"/>
/// which are persisted in a follow-up SaveChanges. Each save has its own captured batch so
/// handlers can safely trigger additional saves.
/// </summary>
public class EntitySaveChangesInterceptor(IServiceProvider serviceProvider) : SaveChangesInterceptor
{
    private readonly ConditionalWeakTable<DbContext, Stack<EntityChanges>> pendingChanges = new();

    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        CaptureEntities(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        CaptureEntities(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public override int SavedChanges(SaveChangesCompletedEventData eventData, int result)
    {
        DispatchAsync(eventData.Context, CancellationToken.None).GetAwaiter().GetResult();
        return base.SavedChanges(eventData, result);
    }

    public override async ValueTask<int> SavedChangesAsync(
        SaveChangesCompletedEventData eventData,
        int result,
        CancellationToken cancellationToken = default)
    {
        await DispatchAsync(eventData.Context, cancellationToken);
        return await base.SavedChangesAsync(eventData, result, cancellationToken);
    }

    public override void SaveChangesFailed(DbContextErrorEventData eventData)
    {
        DiscardEntities(eventData.Context);
        base.SaveChangesFailed(eventData);
    }

    public override Task SaveChangesFailedAsync(
        DbContextErrorEventData eventData,
        CancellationToken cancellationToken = default)
    {
        DiscardEntities(eventData.Context);
        return base.SaveChangesFailedAsync(eventData, cancellationToken);
    }

    public override void SaveChangesCanceled(DbContextEventData eventData)
    {
        DiscardEntities(eventData.Context);
        base.SaveChangesCanceled(eventData);
    }

    public override Task SaveChangesCanceledAsync(
        DbContextEventData eventData,
        CancellationToken cancellationToken = default)
    {
        DiscardEntities(eventData.Context);
        return base.SaveChangesCanceledAsync(eventData, cancellationToken);
    }

    private void CaptureEntities(DbContext? context)
    {
        if (context is null)
        {
            return;
        }

        List<object> addedEntities = context.ChangeTracker.Entries()
            .Where(x => x.State == EntityState.Added)
            .Select(x => x.Entity)
            .ToList();

        List<object> deletedEntities = context.ChangeTracker.Entries()
            .Where(x => x.State == EntityState.Deleted)
            .Select(x => x.Entity)
            .ToList();

        pendingChanges.GetOrCreateValue(context).Push(new EntityChanges(addedEntities, deletedEntities));
    }

    private async Task DispatchAsync(DbContext? context, CancellationToken cancellationToken)
    {
        EntityChanges? changes = TakeEntities(context);
        if (changes is null)
        {
            return;
        }

        foreach (object entity in changes.AddedEntities)
        {
            await InvokeCreatedHandlers(entity, context!, cancellationToken);
        }

        foreach (object entity in changes.DeletedEntities)
        {
            await InvokeDeletedHandlers(entity, context!, cancellationToken);
        }

        if (context!.ChangeTracker.HasChanges())
        {
            await context.SaveChangesAsync(cancellationToken);
        }
    }

    private EntityChanges? TakeEntities(DbContext? context)
    {
        if (context is null || !pendingChanges.TryGetValue(context, out Stack<EntityChanges>? changes) ||
            changes.Count == 0)
        {
            return null;
        }

        return changes.Pop();
    }

    private void DiscardEntities(DbContext? context)
    {
        TakeEntities(context);
    }

    private Task InvokeCreatedHandlers(object entity, DbContext context, CancellationToken cancellationToken)
    {
        Type handlerType = typeof(IEntityCreatedHandler<>).MakeGenericType(entity.GetType());
        return InvokeHandlers(entity, context, cancellationToken, handlerType);
    }

    private Task InvokeDeletedHandlers(object entity, DbContext context, CancellationToken cancellationToken)
    {
        Type handlerType = typeof(IEntityDeletedHandler<>).MakeGenericType(entity.GetType());
        return InvokeHandlers(entity, context, cancellationToken, handlerType);
    }

    private async Task InvokeHandlers(
        object entity,
        DbContext context,
        CancellationToken cancellationToken,
        Type handlerType)
    {
        Type enumerableType = typeof(IEnumerable<>).MakeGenericType(handlerType);

        if (serviceProvider.GetService(enumerableType) is not IEnumerable handlers)
        {
            return;
        }

        MethodInfo? handleMethod = handlerType.GetMethod(nameof(IEntityCreatedHandler<object>.Handle));
        if (handleMethod is null)
        {
            return;
        }

        foreach (object? handler in handlers)
        {
            if (handler is null)
            {
                continue;
            }

            object? task = handleMethod.Invoke(handler, [entity, context, cancellationToken]);
            if (task is Task t)
            {
                await t;
            }
        }
    }

    private sealed record EntityChanges(List<object> AddedEntities, List<object> DeletedEntities);
}
