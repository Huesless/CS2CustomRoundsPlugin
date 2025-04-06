using CounterStrikeSharp.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace CS2CustomRoundsPlugin
{
    public class TPlantAnywhereRound : CustomRound
    {
        public override string RoundStartMessage => "Rush T Spawn!";
        public override string RoundStartDescription => "Ts can plant the bomb anywhere. CTs have 2x time to defuse.";
        public override void RoundEnd()
        {
            Server.ExecuteCommand("mp_c4timer 40");
            Server.ExecuteCommand("mp_plant_c4_anywhere false");
        }

        public override void RoundStart()
        {
            Server.ExecuteCommand("mp_c4timer 80");
            Server.ExecuteCommand("mp_plant_c4_anywhere true");
        }
    }
}
