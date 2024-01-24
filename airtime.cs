using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;

namespace Airtime
{
    public class Airtime : BasePlugin
    {
        public override string ModuleName => "Airtime";
        public override string ModuleVersion => "1.0.2";
        public override string ModuleAuthor => "MLGSW4G (https://github.com/MLGSW4G/)";
        public override string ModuleDescription => "Displays the time smoke grenades travel before detonating.";


        private static bool isEnabled = true;
        readonly List<double> smokeThrownTimes = new();
        readonly List<double> smokeDetonateTimes = new();


        [GameEventHandler]
        public HookResult OnSmokeThrown(EventGrenadeThrown @event, GameEventInfo info)
        {
            if (@event.Userid.IsValid && isEnabled && @event.Weapon == "smokegrenade")
            {
                smokeThrownTimes.Add(NativeAPI.GetEngineTime());
            }

            return HookResult.Continue;
        }

        [GameEventHandler]
        public HookResult OnSmokeDetonate(EventSmokegrenadeDetonate @event, GameEventInfo info)
        {
            if (@event.Userid.IsValid && isEnabled)
            {
                smokeDetonateTimes.Add(NativeAPI.GetEngineTime());
                double airtime = smokeDetonateTimes[0] - smokeThrownTimes[0];
                @event.Userid.PrintToChat($"[{ChatColors.Green}Airtime{ChatColors.Default}] Airtime of smoke grenade: {airtime:0.00} seconds");
                smokeThrownTimes.Remove(smokeThrownTimes[0]);
                smokeDetonateTimes.Remove(smokeDetonateTimes[0]);
            }

            return HookResult.Continue;
        }

        [ConsoleCommand("css_airtime", "Enable/disable smoke grenades airtime display.")]
        public void OnCommand(CCSPlayerController? player, CommandInfo command)
        {
            isEnabled = !isEnabled;
            Server.PrintToChatAll($"[{ChatColors.Green}Airtime{ChatColors.Default}] Smoke grenades airtime display " + (isEnabled ? "enabled." : "disabled."));
        }
    }
}
