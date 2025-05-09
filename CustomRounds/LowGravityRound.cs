using CounterStrikeSharp.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS2CustomRoundsPlugin
{
    public class LowGravityRound : CustomRound
    {
        public override string RoundStartMessage => "Moon gravity";
        public override string RoundStartDescription => "Low gravity with super jump. No weapon spread penalty.";
        public override void RoundStart()
        {
            Server.ExecuteCommand("sv_gravity 260");
            Server.ExecuteCommand("sv_jump_impulse 400");
            Server.ExecuteCommand("weapon_accuracy_nospread 1");
        }
        public override void RoundEnd()
        {
            Server.ExecuteCommand("sv_gravity 800");
            Server.ExecuteCommand("sv_jump_impulse 301.993378");
            Server.ExecuteCommand("weapon_accuracy_nospread 0");
        }

    }
}
