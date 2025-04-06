using CounterStrikeSharp.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS2CustomRoundsPlugin
{
    public class BigPlayersRound : CustomRound
    {
        public override string RoundStartMessage => "BIG MODE";
        public override string RoundStartDescription => "Everyone is BIG.";
        public override void RoundStart()
        {
            Server.ExecuteCommand("c_entfire player SetScale 2");
        }
        public override void RoundEnd()
        {
            Server.ExecuteCommand("c_entfire player SetScale 1");
        }

    }
}
