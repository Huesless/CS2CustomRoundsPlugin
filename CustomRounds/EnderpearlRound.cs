using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS2CustomRoundsPlugin
{
    public class EnderpearlRound : CustomRound
    {
        public override string RoundStartMessage => "Enderpearl";
        public override string RoundStartDescription => $"When thrown decoys land and start firing, teleport to their location. You can buy more decoys.";
        public override void RoundEnd()
        {
            Server.ExecuteCommand("ammo_grenade_limit_default 1");
            Server.ExecuteCommand("ammo_grenade_limit_total 4");
        }

        public override void RoundStart()
        {
            Server.ExecuteCommand("ammo_grenade_limit_default 5");
            Server.ExecuteCommand("ammo_grenade_limit_total 8");
        }

        public override void DecoyStarted(EventDecoyStarted @event)
        {
            var player = @event.Userid;
            if(player != null)
            {
                var pawn = player.PlayerPawn.Get();
                if (pawn != null)
                {
                    pawn.Teleport(new Vector(@event.X, @event.Y, @event.Z));
                }
            }

        }
    }
}
