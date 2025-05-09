using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Entities.Constants;
using CounterStrikeSharp.API.Modules.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Vector = CounterStrikeSharp.API.Modules.Utils.Vector;

namespace CS2CustomRoundsPlugin
{
    public class RocketJumpRound : CustomRound
    {
        public override string RoundStartMessage => "Rocket Jump";
        public override string RoundStartDescription => "HE grenades have insane knockback, lower damage. Infinite grenades. No falldamage.";
        public override void RoundEnd()
        {
            Server.ExecuteCommand("sv_infinite_ammo 0");
            Server.ExecuteCommand("sv_hegrenade_damage_multiplier 1");
            Server.ExecuteCommand("sv_falldamage_scale 1");
        }

        public override void RoundStart()
        {
            GiveHE();
            Server.ExecuteCommand("sv_infinite_ammo 2");
            Server.ExecuteCommand("sv_hegrenade_damage_multiplier 0.1");
            Server.ExecuteCommand("sv_falldamage_scale 0");
        }

        private Dictionary<int, Vector> grenadeDetonateVectors = new Dictionary<int, Vector>();
        public override void HegrenadeDetonate(EventHegrenadeDetonate @event)
        {
            var playerId = @event.Userid?.UserId;
            if(playerId != null)
            {
                Vector detonateLocation = new Vector(@event.X, @event.Y, @event.Z);
                grenadeDetonateVectors[playerId.Value] = detonateLocation;
            }
        }

        public override void PlayerHurt(EventPlayerHurt @event)
        {
            var attackerId = @event.Attacker?.UserId;
            var player = @event.Userid;
            var weapon = @event.Weapon;
            if (attackerId != null && player != null && weapon == "hegrenade")
            {
                var pawn = player.PlayerPawn.Get();
                if (pawn != null)
                {

                    //pawn.TimeScale = 0.5f;
                    //pawn.GravityScale = 0.5f;
                    //pawn.BlindStartTime = (float)Server.TickedTime;
                    //pawn.BlindUntilTime = (float)Server.TickedTime + 5;
                    //pawn.FlashDuration = 5f;
                    //pawn.FlashMaxAlpha = 255;
                    var directionVec = pawn.AbsOrigin! - grenadeDetonateVectors[attackerId.Value];
                    if (directionVec.Z < 0.275)
                    {
                        directionVec.Z = 0.275f;
                    }
                    Vector3 normDirVec = Vector3.Normalize(new Vector3(directionVec.X, directionVec.Y, directionVec.Z));
                    var force = 1500;
                    pawn!.AbsVelocity.X += normDirVec.X * force;
                    pawn.AbsVelocity.Y += normDirVec.Y * force;
                    pawn.AbsVelocity.Z += normDirVec.Z * force;
                }
            }
        }
        private void GiveHE()
        {
            foreach (var player in Utilities.GetPlayers())
            {
                if (player == null || !player.IsValid || player.UserId == null)
                    continue;
                player.GiveNamedItem(CsItem.HEGrenade);
            }
        }
    }
}
