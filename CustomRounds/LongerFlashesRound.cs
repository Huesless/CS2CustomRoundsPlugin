using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS2CustomRoundsPlugin
{
    public class LongerFlashesRound : CustomRound
    {

        public override string RoundStartMessage => "Longer Flashes";
        public override string RoundStartDescription => $"Flash duration is {Multiplier}x longer";
        private float Multiplier = 2;
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
            if(player != null)
            {
                var pawn = player!.PlayerPawn.Get();
                if(pawn != null)
                {
                    pawn.BlindStartTime = (float)Server.TickedTime;
                    pawn.BlindUntilTime = (float)Server.TickedTime + blindDuration*Multiplier;
                    pawn.FlashDuration = blindDuration * Multiplier;
                    pawn.FlashMaxAlpha = 255;
                }

            }


        }
    }
}
