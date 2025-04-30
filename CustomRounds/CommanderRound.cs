using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Entities.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS2CustomRoundsPlugin
{
    public class CommanderRound : CustomRound
    {
        public override bool BuyAllowed => false;
        public override string RoundStartMessage => "Commander mode";
        public override string RoundStartDescription => "Only bots can fight. Command them to victory.";
        public override void RoundEnd()
        {
            Server.ExecuteCommand("bot_kick all");
            Server.ExecuteCommand("mp_death_drop_gun 1");
        }

        public override void RoundStart()
        {
            //Server.ExecuteCommand("bot_quota 0");
            Server.ExecuteCommand("bot_quota_mode normal");
            Server.ExecuteCommand("mp_limitteams 5");
            Server.ExecuteCommand("mp_autoteambalance 0");
            Server.ExecuteCommand("mp_death_drop_gun 0");
            //for (int i = 0; i < 5; i++)
            //{
            //    Server.PrintToChatAll("Adding bots.");
            //    Server.ExecuteCommand("bot_add_ct");
            //    Server.ExecuteCommand("bot_add_t");
            //}
        }

        public override void FreezeTimeEnd(EventRoundFreezeEnd @event)
        {
            for (int i = 0; i < 5; i++)
            {
                Server.ExecuteCommand("bot_add_ct expert");
                Server.ExecuteCommand("bot_add_t expert");
            }
            foreach (var player in Utilities.GetPlayers())
            {
                if (player == null || !player.IsValid)
                    continue;
                player.Respawn();
                if (!player.IsBot)
                {
                    CommonFunc.RemoveWeapons(player);
                }
                
                
            }
        }

    }
}
