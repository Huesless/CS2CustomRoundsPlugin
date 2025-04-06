using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Entities;
using CounterStrikeSharp.API.Modules.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS2CustomRoundsPlugin
{
    public class TPOnHitRound : CustomRound
    {
        public override string RoundStartDescription => "When you get hit, you are randomly teleported.";
        public override string RoundStartMessage => "TP on getting hit";
        public override void RoundStart()
        {
            return;
        }
        public override void RoundEnd()
        {
            return;
        }

        public override void PlayerHurt(EventPlayerHurt @event)
        {
            
            var player = @event.Userid;
            if (player != null)
            {
                var pawn = player.PlayerPawn.Get();

                if (pawn != null)
                {
                    var coords = ValidMapCoordinates.Coords;
                    Random random = new Random();
                    List<double> coord = coords[random.Next(0, coords.Count())];
                    Vector vector = new Vector((float)coord[0], (float)coord[1], (float)coord[2] + 20);
                    pawn.Teleport(vector);
                }
            }
        }

        public override string ToString()
        {
            return "TP on hit";
        }
    }


}
