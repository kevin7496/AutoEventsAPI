# AutoEventsAPI
## This plugin provides a convenient and secure API for creating and automatically uploading auto-events to the server.

To create your first auto event, you need to create a class and inherit from the AutoEvent class. After that, you need to fill in the event parameters.

▶️ In the "Start" method, you need to write the logic for what will happen after the event starts: role distribution, etc.

⛔ The "Stop" method - in it you need to end the event and preferably move players to spectators.

🕒 The "Update" method - called every frame, which allows you to constantly check some data.

```csharp
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
```
