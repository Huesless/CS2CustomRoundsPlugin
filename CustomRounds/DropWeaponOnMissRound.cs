using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS2CustomRoundsPlugin
{
    public class DropWeaponOnMissRound : CustomRound
    {
        private HashSet<nint> shotsHit = new();
        public override string RoundStartDescription => "If you miss a shot, you drop your weapon";
        public override string RoundStartMessage => "Butter fingers";
        public override void RoundEnd()
        {
            return;
        }

        public override void RoundStart()
        {
            return;
        }


        

        public override void PlayerHurt(EventPlayerHurt @event)
        {            
            var player = @event.Attacker;
            if (player != null)
            {
                shotsHit.Add(player.Handle);
            }
        }
        public override void WeaponFire(EventWeaponFire @event)
        {
            var player = @event.Userid;

            if (player != null)
            {
                shotsHit.Remove(player.Handle);
                Server.NextFrame(() =>
                {
                    if (!shotsHit.Contains(player.Handle))
                    {
                        
                        var activeWeapon = player.PlayerPawn.Value!.WeaponServices!.ActiveWeapon.Value;
                        var pos = activeWeapon!.AbsOrigin;
                        player.DropActiveWeapon();
                        if(pos != null)
                        {
                            activeWeapon.Teleport(pos + new Vector(0, 0, 50));
                        }

                    }
                });
            }
        }
    }
}
