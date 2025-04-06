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
    public class RandomSpawnRound : CustomRound
    {
        public override void RoundStart()
        {

            Server.ExecuteCommand("mp_buy_anywhere 1");
            var coords = ValidMapCoordinates.Coords.Select(innerList => new List<double>(innerList)).ToList();
            if (coords != null) 
            {
                foreach (var player in Utilities.GetPlayers())
                {
                    if (player == null || !player.IsValid)
                        continue;
                    var pawn = player.PlayerPawn.Get();

                    if (pawn != null)
                    {

                        Random random = new Random();
                        List<double> coord = coords[random.Next(0, coords.Count())];
                        Vector vector = new Vector((float)coord[0], (float)coord[1], (float)coord[2]+20);
                        coords.Remove(coord);
                        pawn.Teleport(vector);
                    }
                }
            }


            

        }
        public override void RoundEnd()
        {
            Server.ExecuteCommand("mp_buy_anywhere 0");
        }

        public override string RoundStartDescription => "Random spawns.";
        

        public override string RoundStartMessage => "Random spawns";
        
    }
}
