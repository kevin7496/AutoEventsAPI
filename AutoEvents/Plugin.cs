using AutoEvents.Features;
using Exiled.API.Features;
using Exiled.API.Interfaces;

namespace AutoEvents
{
    public class Plugin : Plugin<Config>
    {
        public override string Author => "kevin";
        public override string Prefix => "AutoEvents";
        public override string Name => "AutoEvents";

        public override void OnEnabled()
        {
            AutoEventManager.Awake();

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            base.OnDisabled();
        }
    }

    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; } = false;
    }
}
