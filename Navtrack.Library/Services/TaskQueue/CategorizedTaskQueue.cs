using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Navtrack.Library.Services.TaskQueue
{
    public sealed class CategorizedTaskQueue<TCategory> : IDisposable
    {
        private readonly ConcurrentDictionary<TCategory, SemaphoreSlim> categorySemaphores =
            new ConcurrentDictionary<TCategory, SemaphoreSlim>();

        private readonly TaskQueue queue;

        public CategorizedTaskQueue() : this(degreesOfParallelism: 1)
        {
        }

        public CategorizedTaskQueue(int degreesOfParallelism)
        {
            queue = new TaskQueue(degreesOfParallelism);
        }

        public async Task<T> Enqueue<T>(TCategory category, Func<Task<T>> taskGenerator, CancellationToken token)
        {
            SemaphoreSlim myCategorySemaphore = categorySemaphores.GetOrAdd(category, _ => new SemaphoreSlim(1));
            await myCategorySemaphore.WaitAsync(token);
            try
            {
                return await queue.Enqueue(taskGenerator, token);
            }
            finally
            {
                myCategorySemaphore.Release();
            }
        }

        public Task<T> Enqueue<T>(TCategory category, Func<T> operation, CancellationToken token)
        {
            return Enqueue(category, () => Task.Run(operation, token), token);
        }

        public Task<T> Enqueue<T>(TCategory category, Func<T> operation)
        {
            return Enqueue(category, () => Task.Run(operation), CancellationToken.None);
        }

        public async Task Enqueue(TCategory category, Func<Task> taskGenerator, CancellationToken token)
        {
            SemaphoreSlim myCategorySemaphore = categorySemaphores.GetOrAdd(category, _ => new SemaphoreSlim(1));
            await myCategorySemaphore.WaitAsync(token);
            try
            {
                await queue.Enqueue(taskGenerator, token);
            }
            finally
            {
                myCategorySemaphore.Release();
            }
        }

        public Task Enqueue(TCategory category, Action operation, CancellationToken token)
        {
            return Enqueue(category, () => Task.Run(operation, token), token);
        }

        public Task Enqueue(TCategory category, Action operation)
        {
            return Enqueue(category, () => Task.Run(operation), CancellationToken.None);
        }

        public void Dispose()
        {
            queue.Dispose();

            foreach (SemaphoreSlim semaphore in categorySemaphores.Values)
            {
                semaphore.Dispose();
            }
        }
    }
}