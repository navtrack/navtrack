using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Navtrack.Database.Interceptors;

/// <summary>
/// Captures entities transitioning to the Added state during SaveChanges and, after the save
/// succeeds, dispatches them to all registered <see cref="IEntityCreatedHandler{TEntity}"/>
/// implementations. Handlers may stage additional changes on the same <see cref="DbContext"/>
/// which are persisted in a follow-up SaveChanges. A re-entrancy guard prevents infinite
/// recursion when handlers themselves trigger saves.
/// </summary>
public class EntitySaveChangesInterceptor(IServiceProvider serviceProvider) : SaveChangesInterceptor
{
    private List<object> addedEntities = [];
    private bool isProcessing;

    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        CaptureAddedEntities(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        CaptureAddedEntities(eventData.Context);
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

    private void CaptureAddedEntities(DbContext? context)
    {
        if (context is null || isProcessing)
        {
            return;
        }

        addedEntities = context.ChangeTracker.Entries()
            .Where(x => x.State == EntityState.Added)
            .Select(x => x.Entity)
            .ToList();
    }

    private async Task DispatchAsync(DbContext? context, CancellationToken cancellationToken)
    {
        if (context is null || isProcessing || addedEntities.Count == 0)
        {
            return;
        }

        List<object> entities = addedEntities;
        addedEntities = [];
        isProcessing = true;

        try
        {
            foreach (object entity in entities)
            {
                await InvokeHandlers(entity, context, cancellationToken);
            }

            if (context.ChangeTracker.HasChanges())
            {
                await context.SaveChangesAsync(cancellationToken);
            }
        }
        finally
        {
            isProcessing = false;
        }
    }

    private async Task InvokeHandlers(object entity, DbContext context, CancellationToken cancellationToken)
    {
        Type handlerType = typeof(IEntityCreatedHandler<>).MakeGenericType(entity.GetType());
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
}
