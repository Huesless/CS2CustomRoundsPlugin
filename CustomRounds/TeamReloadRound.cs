using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS2CustomRoundsPlugin
{
    public class TeamReloadRound : CustomRound
    {
        public override void RoundEnd()
        {
            return;
        }

        public override void RoundStart()
        {
            return;
        }

        public override string RoundStartDescription => "When a player reloads, everyone on the team reloads.";

        public override string RoundStartMessage => "All for One";

        public override void PlayerReload(EventWeaponReload @event)
        {
            var reloadPlayer = @event.Userid;
            if (reloadPlayer != null)
            {
                var teamNum = reloadPlayer.TeamNum;
                foreach (var player in Utilities.GetPlayers().Where(x => x.TeamNum == teamNum))
                {
                    if (player == null || !player.IsValid)
                        continue;

                    var pawn = player.PlayerPawn.Get();
                    var activeWeapon = pawn?.WeaponServices?.ActiveWeapon.Get();
                    if (activeWeapon != null)
                    {
                        activeWeapon.Clip1 = 0;
                    }

                }
            }
            
            
            
        }
    }
}
