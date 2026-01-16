using AutoEvents.Features;
using CommandSystem;
using System;
using System.Linq;

namespace AutoEvents.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(Parent))]
    public class Start : ICommand
    {
        public string Command => "start";

        public string[] Aliases => new string[] { };

        public string Description => "Запустить авто ивент";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (arguments.Count < 1)
            {
                response = "Недостаточно аргументов. ae start <айди>";
                return false;
            }
            if (!int.TryParse(arguments.First(), out int value))
            {
                response = "Некоректный айди";
                return false;
            }

            return AutoEventManager.StartEvent(value, out response);
        }
    }
}
