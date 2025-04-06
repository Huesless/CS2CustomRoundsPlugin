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
    public class ReloadSwitchRound : CustomRound
    {
        public override bool BuyAllowed => false;
        public override string RoundStartMessage => "Weapon roulette 2";
        public override string RoundStartDescription => "When you manually reload get a new random gun.";
        public override void RoundStart()
        {
            return;
        }
        public override void RoundEnd()
        {
            return;
        }

        public override void PlayerReload(EventWeaponReload @event)
        {
            var player = @event.Userid;
            if (player != null)
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

        public override void WeaponFireEmpty(EventWeaponFireOnEmpty @event)
        {
            
            var player = @event.Userid;
            if (player != null)
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
