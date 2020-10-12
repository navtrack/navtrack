using System;
using System.Threading;
using System.Threading.Tasks;

namespace Navtrack.Library.Services.TaskQueue
{
    public sealed class TaskQueue : IDisposable
    {
        private readonly SemaphoreSlim semaphore;

        public TaskQueue() : this(degreesOfParallelism: 1)
        { }

        public TaskQueue(int degreesOfParallelism)
        {
            semaphore = new SemaphoreSlim(degreesOfParallelism);
        }

        public async Task<T> Enqueue<T>(Func<Task<T>> taskGenerator, CancellationToken token)
        {
            await semaphore.WaitAsync(token);
            try
            {
                return await taskGenerator();
            }
            finally
            {
                semaphore.Release();
            }
        }

        public async Task Enqueue(Func<Task> taskGenerator, CancellationToken token)
        {
            await semaphore.WaitAsync(token);
            try
            {
                await taskGenerator();
            }
            finally
            {
                semaphore.Release();
            }
        }

        public void Dispose()
        {
            semaphore.Dispose();
        }
    }
}