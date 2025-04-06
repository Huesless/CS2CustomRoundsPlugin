using CounterStrikeSharp.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS2CustomRoundsPlugin
{
    public class IceFloorRound : CustomRound
    {
        public override string RoundStartMessage => "Slip 'n slide";
        public override string RoundStartDescription => "The floor is ice.";
        public override void RoundStart()
        {
            Server.ExecuteCommand("sv_friction 0.5");
        }
        public override void RoundEnd()
        {
            Server.ExecuteCommand("sv_friction 5.2");
        }



    }
}
