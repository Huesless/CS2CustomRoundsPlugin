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
    public class KnockBackRound : CustomRound
    {
        public override string RoundStartMessage => "Jetpack joyride!";
        public override string RoundStartDescription => "Weapons have insane knockback.";
        public override void RoundEnd()
        {
            return;
        }

        public override void RoundStart()
        {
            return;
        }

        public override void WeaponFire(EventWeaponFire @event)
        {
            var player = @event.Userid;
            var pawn = player!.PlayerPawn.Get();
            var weapon = @event.Weapon;
            var force = 200;
            if (WeaponsSet.BigKnockback.Contains(weapon))
            {
                force = 1200;
            }

            var directionVec = new Vector();
            NativeAPI.AngleVectors(pawn!.EyeAngles.Handle, directionVec.Handle, IntPtr.Zero,
                IntPtr.Zero);

            pawn!.AbsVelocity.X -= directionVec.X * force;
            pawn.AbsVelocity.Y -= directionVec.Y * force;
            pawn.AbsVelocity.Z -= directionVec.Z * force;
        }
    }
}
