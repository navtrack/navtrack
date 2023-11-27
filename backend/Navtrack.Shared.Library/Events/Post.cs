using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Shared.Library.Events;

[Service(typeof(IPost))]
public class Post : IPost
{
    private readonly IServiceProvider serviceProvider;

    public Post(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    public Task Send<T>(T payload)
    {
        IEnumerable<IEventHandler<T>> eventHandlers = serviceProvider.GetServices<IEventHandler<T>>();
        
        return Task.WhenAll(eventHandlers.Select(x => x.Handle(payload)));
    }
}