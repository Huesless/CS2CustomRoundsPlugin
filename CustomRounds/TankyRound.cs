using CounterStrikeSharp.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS2CustomRoundsPlugin
{
    public class TankyRound : CustomRound
    {
        public override void RoundStart()
        {
            foreach (var player in Utilities.GetPlayers())
            {
                if (player == null || !player.IsValid)
                    continue;
                var pawn = player.PlayerPawn.Get();

                if (pawn != null)
                {
                    pawn.MaxHealth = 500;
                    pawn.Health = 500;

                }
            }
        }
        public override void RoundEnd()
        {
            foreach (var player in Utilities.GetPlayers())
            {
                if (player == null || !player.IsValid)
                    continue;
                var pawn = player.PlayerPawn.Get();

                if (pawn != null)
                {
                    pawn.MaxHealth = 100;
                    pawn.Health = 100;

                }
            }
        }

        public override string RoundStartDescription => "Everyone is tanky.";
        

        public override string RoundStartMessage => "Chonky";
        
    }
}
