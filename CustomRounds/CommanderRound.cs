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
        public override bool BuyAllowed => true;
        public override string RoundStartMessage => "Everyone, get in here!";
        public override string RoundStartDescription => "Each team gets a reinforcement of bots.";
        public override void RoundEnd()
        {
            for (int i = 0; i < 15; i++)
            {
                Server.ExecuteCommand("bot_kick ct");
                Server.ExecuteCommand("bot_kick t");
            }
            //Server.ExecuteCommand("bot_kick all");
        }

        public override void RoundStart()
        {
            //Server.ExecuteCommand("bot_quota 0");
            Server.ExecuteCommand("bot_quota_mode fill");
            Server.ExecuteCommand("mp_limitteams 5");
            Server.ExecuteCommand("mp_autoteambalance 0");

        }

        public override void FreezeTimeEnd(EventRoundFreezeEnd @event)
        {
            for (int i = 0; i < 15; i++)
            {
                Server.ExecuteCommand("bot_add_ct expert");
                Server.ExecuteCommand("bot_add_t expert");
            }
            //foreach (var player in Utilities.GetPlayers())
            //{
            //    if (player == null || !player.IsValid)
            //        continue;
            //    player.Respawn();
            //    if (!player.IsBot)
            //    {
            //        CommonFunc.RemoveWeapons(player);
            //    }
                
                
            //}
        }

    }
}
