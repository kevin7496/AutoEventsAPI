using CommandSystem;
using System;
using System.Text;

namespace AutoEvents.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(Parent))]
    public class List : ICommand
    {
        public string Command => "list";

        public string[] Aliases => new string[] { "l" };

        public string Description => "Список авто ивентов";

        public static string EventsList = "Список авто ивентов";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            response = EventsList;
            return true;
        }
    }
}
