using System;
using Navtrack.Library.DI;

namespace Navtrack.Listener.Services
{
    [Service(typeof(IListener))]
    internal class Listener : IListener
    {
        public void Listen()
        {
            Console.WriteLine("Started to listen :)");
        }
    }
}