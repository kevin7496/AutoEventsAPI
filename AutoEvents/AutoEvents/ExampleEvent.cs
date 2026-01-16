using Exiled.API.Features;
using System;

namespace AutoEvents.AutoEvents
{
    internal class ExampleEvent : AutoEvent
    {
        public override string Name => "ExampleEvent";

        public override string Description => "ExampleEvent";

        public override uint MinPlayers => 1;

        public override void Start()
        {
            foreach (Player player in Player.List)
            {
                player.Role.Set(PlayerRoles.RoleTypeId.Tutorial);
            }
        }

        public override void Stop()
        {
            foreach (Player player in Player.List)
            {
                player.Role.Set(PlayerRoles.RoleTypeId.Spectator);
            }
        }

        protected override void Update()
        {
            foreach (Player player in Player.List)
            {
                player.ShowHint($"{player.Nickname}", 0.1f);
            }
        }
    }
}
