using CounterStrikeSharp.API.Modules.Entities.Constants;
using CounterStrikeSharp.API.Modules.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CounterStrikeSharp.API;

namespace CS2CustomRoundsPlugin
{
    public class HEOnlyRound : CustomRound
    {
        public override bool BuyAllowed => false;
        public override void RoundStart()
        {
            
            Server.ExecuteCommand("sv_infinite_ammo 2");

            foreach (var player in Utilities.GetPlayers())
            {
                if (player == null || !player.IsValid)
                    continue;
                var pawn = player.PlayerPawn.Get();

                if (pawn != null)
                {
                    pawn.MaxHealth = 200;
                    pawn.Health = 200;
                }

                CommonFunc.RemoveWeapons(player);
                player.GiveNamedItem(CsItem.HEGrenade);
                player.GiveNamedItem(CsItem.Knife);
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
            
            Server.ExecuteCommand("mp_buytime 20");
            Server.ExecuteCommand("sv_infinite_ammo 0");
        }


        public override string RoundStartDescription => "HE grenades only.";
        

        public override string RoundStartMessage => "KOBE KOBE KOBE";
        
    }
}
