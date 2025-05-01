using CounterStrikeSharp.API.Modules.Entities.Constants;
using CounterStrikeSharp.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CounterStrikeSharp.API.Core;

namespace CS2CustomRoundsPlugin
{
    public class InvertedControlsRound : CustomRound
    {
        public override string RoundStartMessage => "Inverted controls";
        public override string RoundStartDescription => "Movement keys are inverted";
        public override void RoundEnd()
        {
            UnInvertControls();
        }

        public override void RoundStart()
        {
            
        }
        public override void FreezeTimeEnd(EventRoundFreezeEnd @event)
        {
            InvertControls();
        }
        public void InvertControls()
        {
            foreach (var player in Utilities.GetPlayers())
            {
                if (player == null || !player.IsValid)
                    continue;
                Server.PrintToChatAll("invert");
                player.ExecuteClientCommand("bind s +forward");
                player.ExecuteClientCommand("bind w +back");
                player.ExecuteClientCommand("bind d +moveleft");
                player.ExecuteClientCommand("bind a +moveright");

            }
        }
        public void UnInvertControls()
        {
            foreach (var player in Utilities.GetPlayers())
            {
                if (player == null || !player.IsValid)
                    continue;
                player.ExecuteClientCommand("bind w +forward");
                player.ExecuteClientCommand("bind s +back");
                player.ExecuteClientCommand("bind a +moveleft");
                player.ExecuteClientCommand("bind d +moveright");

            }
        }
    }
}
