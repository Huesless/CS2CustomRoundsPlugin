using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS2CustomRoundsPlugin
{
    public abstract class CustomRound
    {
        public virtual bool BuyAllowed => true;
        public virtual string RoundStartMessage => "";
        public virtual string RoundStartDescription => "";
        public abstract void RoundStart();
        public abstract void RoundEnd();
        

        public virtual void BombPlant(CCSPlayerController player)
        {
            return;
        }

        public virtual void PlayerReload(EventWeaponReload @event)
        {
            return;
        }

        public virtual void PlayerHurt(EventPlayerHurt @event)
        {
            return;
        }

        public virtual void PlayerDeath(EventPlayerDeath @event)
        {
            return;
        }

        public virtual void WeaponFire(EventWeaponFire @event)
        {
            return;
        }
        public virtual void PlayerSpawn(EventPlayerSpawn @event)
        {
            return;
        }
        public virtual void BulletImpact(EventBulletImpact @event)
        {
            return;
        }
        public virtual void WeaponFireEmpty(EventWeaponFireOnEmpty @event)
        {
            return;
        }
        public virtual void FreezeTimeEnd(EventRoundFreezeEnd @event)
        {
            return;
        }
        public override string ToString()
        {
            return "Custom round";
        }


    }
}
