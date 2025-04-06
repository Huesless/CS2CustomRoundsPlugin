using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Modules.Entities.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS2CustomRoundsPlugin
{
    public class DeagleHSOnlyRound : CustomRound
    {
        public override bool BuyAllowed => false;
        public override string RoundStartDescription => "Deagle headshot only.";
        public override string RoundStartMessage => "Deagle Headshot Only";
        public override void RoundStart()
        {
            foreach (var player in Utilities.GetPlayers())
            {
                if (player == null || !player.IsValid)
                    continue;


                CommonFunc.RemoveWeapons(player);
                player.GiveNamedItem(CsItem.Deagle);

            }
            Server.ExecuteCommand("mp_damage_headshot_only 1");
            Server.ExecuteCommand("sv_infinite_ammo 2");
        }
        public override void RoundEnd()
        {
            Server.ExecuteCommand("mp_damage_headshot_only 0");
            Server.ExecuteCommand("sv_infinite_ammo 0");
        }
        
    }
}
