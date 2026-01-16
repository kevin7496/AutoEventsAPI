using AutoEvents.Features;
using CommandSystem;
using System;

namespace AutoEvents.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(Parent))]
    public class Stop : ICommand
    {
        public string Command => "stop";

        public string[] Aliases => new string[] { "s" };

        public string Description => "Остановить авто ивент";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            return AutoEventManager.StopEvent(out response);
        }
    }
}
