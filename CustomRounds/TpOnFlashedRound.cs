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
    public class TpOnFlashedRound : CustomRound
    {
        public override string RoundStartMessage => "Hey, you, you're finally awake";
        public override string RoundStartDescription => "Get teleported to a random location when getting flashed.";
        public override void RoundEnd()
        {
            return;
        }

        public override void RoundStart()
        {
            return;
        }
        public override void PlayerBlind(EventPlayerBlind @event)
        {
            var player = @event.Userid;
            var blindDuration = @event.BlindDuration;
            

            Server.PrintToChatAll(blindDuration.ToString());
            if (player != null && blindDuration > 2f)
            {
                var pawn = player.PlayerPawn.Get();

                if (pawn != null)
                {
                    var coords = ValidMapCoordinates.Coords;
                    Random random = new Random();
                    List<double> coord = coords[random.Next(0, coords.Count())];
                    Vector vector = new Vector((float)coord[0], (float)coord[1], (float)coord[2] + 1);
                    pawn.Teleport(vector);
                }
            }
        }
    }
}
