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
        public override string ModuleVersion => "1.0.1";
        public override string ModuleAuthor => "MLGSW4G (https://github.com/MLGSW4G/)";
        public override string ModuleDescription => "Displays the time smoke grenades travel before detonating.";

        
        private static bool isEnabled = true;
        private double smokeThrownTime;
        private double smokeDetonateTime;


        [GameEventHandler]
        public HookResult OnSmokeThrown(EventGrenadeThrown @event, GameEventInfo info)
        {
            if (@event.Userid.IsValid && isEnabled && @event.Weapon == "smokegrenade")
            {
                smokeThrownTime = NativeAPI.GetEngineTime();
            }

            return HookResult.Continue;
        }

        [GameEventHandler]
        public HookResult OnSmokeDetonate(EventSmokegrenadeDetonate @event, GameEventInfo info)
        {
            if (@event.Userid.IsValid && isEnabled)
            {
                smokeDetonateTime = NativeAPI.GetEngineTime();
                double airtime = smokeDetonateTime - smokeThrownTime;
                @event.Userid.PrintToChat($"[{ChatColors.Green}Airtime{ChatColors.Default}] Airtime of smoke grenade: {airtime:0.00} seconds");
            }

            return HookResult.Continue;
        }

        [ConsoleCommand("css_airtime", "Enable/disable smoke grenades airtime display.")]
        public void OnCommand(CCSPlayerController? player, CommandInfo command)
        {
            if (isEnabled)
            {
                Server.PrintToChatAll($"[{ChatColors.Green}Airtime{ChatColors.Default}] Smoke grenades airtime display disabled.");
                isEnabled = false;
            }
            else
            {
                Server.PrintToChatAll($"[{ChatColors.Green}Airtime{ChatColors.Default}] Smoke grenades airtime display enabled.");
                isEnabled = true;
            }
        }
    }
}
