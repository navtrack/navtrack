using Navtrack.Library.DI;

namespace Navtrack.Listener.Protocols.Meiligao
{
    [Service(typeof(IMeiligaoMessageHandler))]
    class MeiligaoMessageHandler : IMeiligaoMessageHandler
    {
        public MeiligaoCommand HandleMessage(MeiligaoMessage message)
        {
            if (message.Command == MeiligaoCommands.Login)
            {
                MeiligaoCommand reply =
                    new MeiligaoCommand(MeiligaoCommands.LoginConfirmation, message.DeviceIdHex, "01");

                return reply;
            }

            return null;
        }
    }
}