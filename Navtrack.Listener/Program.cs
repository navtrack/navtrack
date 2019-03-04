using System;
using Navtrack.Library.DI;
using Navtrack.Listener.Services;

namespace Navtrack.Listener
{
    class Program
    {
        static void Main(string[] args)
        {
            Bootstrapper bootstrapper = new Bootstrapper();
            
            bootstrapper.Initalise();

            IListener listener = bootstrapper.GetService<IListener>();
            
            listener.Listen();
        }
    }
}
