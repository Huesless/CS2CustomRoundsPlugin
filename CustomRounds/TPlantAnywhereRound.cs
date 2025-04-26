using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Modules.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace CS2CustomRoundsPlugin
{
    public class TPlantAnywhereRound : CustomRound
    {
        public override string RoundStartMessage => "Rush T Spawn!";
        public override string RoundStartDescription => "Ts can plant the bomb anywhere. CTs have 2x time to defuse. Random spawns.";
        public override void RoundEnd()
        {
            Server.ExecuteCommand("mp_c4timer 40");
            Server.ExecuteCommand("mp_plant_c4_anywhere false");
            Server.ExecuteCommand("mp_buy_anywhere 0");
        }

        public override void RoundStart()
        {
            Server.ExecuteCommand("mp_c4timer 80");
            Server.ExecuteCommand("mp_plant_c4_anywhere true");
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
                        Vector vector = new Vector((float)coord[0], (float)coord[1], (float)coord[2] + 1);
                        coords.Remove(coord);
                        pawn.Teleport(vector);
                    }
                }
            }
        }
    }
}
