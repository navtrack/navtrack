using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Api.Services.Requests;

[Service(typeof(IRequestHandler))]
public class RequestHandler(IServiceProvider serviceProvider, DbContext dbContext) : IRequestHandler
{
    public Task Handle<TRequest>(TRequest request)
    {
        IRequestHandler<TRequest> handler = serviceProvider.GetRequiredService<IRequestHandler<TRequest>>();

        return ExecuteInTransaction(() => handler.Handle(request));
    }

    public Task<TResult> Handle<TRequest, TResult>(TRequest request)
    {
        IRequestHandler<TRequest, TResult> handler =
            serviceProvider.GetRequiredService<IRequestHandler<TRequest, TResult>>();

        return ExecuteInTransaction(() => handler.Handle(request));
    }

    private Task ExecuteInTransaction(Func<Task> operation)
    {
        return ExecuteInTransaction(async () =>
        {
            await operation();

            return true;
        });
    }

    private async Task<TResult> ExecuteInTransaction<TResult>(Func<Task<TResult>> operation)
    {
        await using IDbContextTransaction transaction = await dbContext.Database.BeginTransactionAsync();

        try
        {
            TResult result = await operation();
            await transaction.CommitAsync();

            return result;
        }
        catch
        {
            await transaction.RollbackAsync();

            throw;
        }
    }
}
