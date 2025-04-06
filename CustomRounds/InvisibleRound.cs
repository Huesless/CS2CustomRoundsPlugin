using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS2CustomRoundsPlugin
{
    public class InvisibleRound : CustomRound
    {
        public override string RoundStartMessage => "Poltergheist";
        public override string RoundStartDescription =>  "Player are fully invisible :).";
        public override void RoundStart()
        {
            Server.ExecuteCommand("mp_buytime 0");
            

        }
        public override void RoundEnd()
        {
            Server.ExecuteCommand("mp_buytime 20");
            Server.ExecuteCommand("c_entfire player alpha 255");
            Server.ExecuteCommand("c_entfire weapon_* alpha 255");
        }

        public override void FreezeTimeEnd(EventRoundFreezeEnd @event)
        {
            Server.ExecuteCommand("c_entfire player alpha 0");
            Server.ExecuteCommand("c_entfire weapon_* alpha 0");
        }

    }
}
