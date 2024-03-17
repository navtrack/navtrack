using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Shared.Library.Events;

[Service(typeof(IPost))]
public class Post(IServiceProvider provider) : IPost
{
    public Task Send<T>(T payload) where T : IEvent
    {
        IEnumerable<IEventHandler<T>> eventHandlers = provider.GetServices<IEventHandler<T>>();
        IEnumerable<IEventHandler> globalEventHandlers = provider.GetServices<IEventHandler>();

        Task[] tasks =
        [
            ..eventHandlers.Select(x => x.Handle(payload)),
            ..globalEventHandlers.Select(x => x.Handle(typeof(T).Name, payload))
        ];

        return Task.WhenAll(tasks);
    }
}