using CounterStrikeSharp.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS2CustomRoundsPlugin
{
    public class BounceRound : CustomRound
    {
        public override string RoundStartMessage => "Bouncy castle";
        public override string RoundStartDescription => "You can bounce off of walls and players by jumping into them.";
        public override void RoundStart()
        {
            Server.ExecuteCommand("sv_bounce 8");
        }
        public override void RoundEnd()
        {
            Server.ExecuteCommand("sv_bounce 0");
        }
    }
}
