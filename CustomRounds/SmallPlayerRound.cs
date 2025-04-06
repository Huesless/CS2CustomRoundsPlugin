using CounterStrikeSharp.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS2CustomRoundsPlugin
{
    public class SmallPlayerRound : CustomRound
    {
        public override string RoundStartMessage => "small mode";
        public override string RoundStartDescription => "Everyone is small.";
        public override void RoundStart()
        {
            Server.ExecuteCommand("c_entfire player SetScale 0.5");
        }
        public override void RoundEnd()
        {
            Server.ExecuteCommand("c_entfire player SetScale 1");
        }
    }
}
