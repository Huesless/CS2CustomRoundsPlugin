using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Entities.Constants;
using CounterStrikeSharp.API.Modules.Timers;
using CounterStrikeSharp.API.Modules.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS2CustomRoundsPlugin
{
    public class OneHPDecoyRound : CustomRound
    {
        public override bool BuyAllowed => false;
        public override string RoundStartMessage => "1HP unlimited decoys";
        public override string RoundStartDescription => "Everyone has 1HP. You have unlimited decoys.";
        public override void RoundStart()
        {
            foreach (var player in Utilities.GetPlayers())
            {
                if (player == null || !player.IsValid)
                    continue;
                var pawn = player.PlayerPawn.Get();

                if (pawn != null)
                {
                    pawn.Health = 1;
                    //pawn!.AddEntityIOEvent("SetHealth", null, null, "1");
                    //Server.PrintToChatAll(player.Health.ToString());
                    //Server.PrintToChatAll(pawn.Health.ToString());
                    //Utilities.SetStateChanged(player, "CBaseEntity", "m_iHealth");
                    //Utilities.SetStateChanged(player.Pawn.Value!, "CBaseEntity", "m_iHealth");
                    //Utilities.SetStateChanged(pawn, "CBaseEntity", "m_iHealth");
                    
                }
            }
            Server.ExecuteCommand("mp_freezetime 0");
            Server.ExecuteCommand("mp_buytime 0");
            Server.ExecuteCommand("sv_infinite_ammo 2");
        }
        public override void RoundEnd()
        {

            Server.ExecuteCommand("mp_freezetime 15");
            Server.ExecuteCommand("mp_buytime 15");
            Server.ExecuteCommand("sv_infinite_ammo 0");
        }

        public override void FreezeTimeEnd(EventRoundFreezeEnd @event)
        {
            foreach (var player in Utilities.GetPlayers())
            {
                if (player == null || !player.IsValid)
                    continue;
                CommonFunc.RemoveWeapons(player);
                player.GiveNamedItem(CsItem.Decoy);
                player.GiveNamedItem(CsItem.Knife);
            }
        }



    }
}
