using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Entities;
using CounterStrikeSharp.API.Modules.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS2CustomRoundsPlugin
{
    public class TPGunRound : CustomRound
    {
        public override void RoundEnd()
        {
            return;
        }

        public override void RoundStart()
        {
            return;
        }

        public override string RoundStartDescription => "Your secondary teleports you.";
        

        public override string RoundStartMessage => "Pocket TP gun";
        

        private HashSet<nint> TPShot = new();

        public override void WeaponFire(EventWeaponFire @event)
        {
            var weapon = @event.Weapon;
            var player = @event.Userid;

            if (player != null && weapon != null && WeaponsSet.SecondaryWeapons.Contains(weapon))
            {
                
                TPShot.Add(player.Handle);
            }
        }

        public override void BulletImpact(EventBulletImpact @event)
        {
            var player = @event.Userid;
            if (player != null)
            {

                if (TPShot.Contains(player.Handle))
                {
                    var pawn = player.PlayerPawn.Get();
                    if (pawn != null)
                    {

                        pawn.Teleport(new Vector(@event.X, @event.Y, @event.Z));
                    }
                    TPShot.Remove(player.Handle);
                }
            }

        }
    }
}
