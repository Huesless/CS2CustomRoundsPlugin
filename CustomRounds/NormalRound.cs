using CounterStrikeSharp.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS2CustomRoundsPlugin
{
    public class NormalRound : CustomRound
    {
        public override void RoundEnd()
        {
            return;
        }

        public override void RoundStart()
        {
            return;
        }

        public override string RoundStartDescription => "This is a normal round.";


        public override string RoundStartMessage => "Normal Round.";
        
    }
}
