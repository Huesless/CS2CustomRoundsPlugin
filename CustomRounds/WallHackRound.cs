using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Modules.Entities.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS2CustomRoundsPlugin
{
    public class WallHackRound : CustomRound
    {
        public override string RoundStartMessage => "True CS2 Experience";
        public override string RoundStartDescription => "Everyone has wallhacks.";
        public override void RoundStart()
        {
            foreach (var player in Utilities.GetPlayers())
            {
                if (player == null || !player.IsValid)
                    continue;
                Server.ExecuteCommand($"c_glow {player.PlayerName}");
            }
        }
        public override void RoundEnd()
        {
            return;
        }

        

        
    }
}
