using CommandSystem;
using Exiled.API.Features;
using System;

namespace AutoEvents.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class Parent : ParentCommand
    {
        public Parent() => LoadGeneratedCommands();

        public override string Command => "autoevent";

        public override string[] Aliases => new string[] { "ae" };

        public override string Description => "Авто ивенты";

        public override void LoadGeneratedCommands()
        {
            Log.Error("ГЕЙ");
        }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            string text = "";
            foreach (var command in Commands)
            {
                text += $"\n{command.Value.Command} ( {string.Join(", ", command.Value.Aliases)} ) - {command.Value.Description}";
            }
            response = text;
            return true;
        }
    }
}
