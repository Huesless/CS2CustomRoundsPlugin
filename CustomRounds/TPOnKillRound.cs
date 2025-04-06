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
    public class TPOnKillRound : CustomRound
    {
        public override string RoundStartDescription => "When you kill someone, teleport to their location";
        public override string RoundStartMessage => "TP on Kill";
        public override void RoundStart()
        {
            return;
        }
        public override void RoundEnd()
        {
            return;
        }
        
        public override void PlayerDeath(EventPlayerDeath @event)
        {
            var player = @event.Userid;
            var attacker = @event.Attacker;
            if (player != null && attacker != null)
            {
                var position = player.PlayerPawn.Get()?.AbsOrigin;
                var attackerPawn = attacker.PlayerPawn.Get();
                attackerPawn?.Teleport(position);

            }
        }
    }
}
