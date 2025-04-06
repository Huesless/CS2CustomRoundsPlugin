using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Entities.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS2CustomRoundsPlugin
{
    public class OneBulletSwitchRound : CustomRound
    {
        public override bool BuyAllowed => false;
        public override string RoundStartMessage => "Weapon roulette";
        public override string RoundStartDescription => "After each fired bullet get a new random gun.";
        public override void RoundStart()
        {
            return;
        }
        public override void RoundEnd()
        {
            return;
        }
        public override void WeaponFire(EventWeaponFire @event)
        {
            var player = @event.Userid;
            var weapon = @event.Weapon;
            if (player != null && weapon != null && WeaponsSet.AllGuns.Contains(weapon))
            {
                var rand = new Random();
                var pawn = player.PlayerPawn.Value;
                if (pawn != null)
                {
                    CommonFunc.RemoveWeapons(player);
                    player.GiveNamedItem(WeaponsSet.AllGuns.ElementAt(rand.Next(WeaponsSet.AllGuns.Count)));
                }

            }
        }
    }
}
