﻿using CounterStrikeSharp.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS2CustomRoundsPlugin
{
    public class SpeedRound : CustomRound
    {
        //TO DO recall on playerhurt
        public override string RoundStartDescription => "Everyone has the zoomies. You are fast.";
        public override string RoundStartMessage => "You are SPEED";
        public override void RoundStart()
        {
            foreach (var player in Utilities.GetPlayers())
            {
                if (player == null || !player.IsValid)
                    continue;
                var pawn = player.PlayerPawn.Get();
                if (pawn != null)
                {
                    pawn.VelocityModifier = 2.5f;
                    
                }
                //Server.ExecuteCommand("sv_accelerate 1000");
                //Server.ExecuteCommand("sv_maxvelocity 9000");
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
                    pawn.VelocityModifier = 1;
                }
                //Server.ExecuteCommand("sv_accelerate 5.5");
                //Server.ExecuteCommand("sv_maxvelocity 3500");
            }
        }

        
    }
}
