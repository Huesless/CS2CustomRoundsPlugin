using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Entities;
using CounterStrikeSharp.API.Modules.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS2CustomRoundsPlugin
{
    public class DashRound : CustomRound
    {
        public override string RoundStartMessage => "Valorant";
        public override string RoundStartDescription => $"Press E to dash in the direction of the crosshair. {cooldown} seconds cooldown.";

        public override void RoundEnd()
        {
            return;
        }

        public override void RoundStart()
        {
            SetupCooldownManager();
        }
        
        private void SetupCooldownManager()
        {
            foreach(var player in Utilities.GetPlayers())
            {
                if(player != null && player.UserId != null)
                {
                    if (!CooldownManager.ContainsKey(player.UserId!.Value))
                    {
                        CooldownManager[player.UserId.Value] = 0;
                    }
                }
                
            }
        }
        private double cooldown = 3;
        private Dictionary<int, double> CooldownManager = new Dictionary<int, double>();
        public override void OnTick()
        {
            foreach (var player in Utilities.GetPlayers())
            {
                if (player != null
                && player.IsValid
                && !player.IsBot
                && player.PawnIsAlive
                && player.UserId != null)
                {
                    var buttons = player.Buttons;
                    var pawn = player.PlayerPawn.Value!;
                    if ((buttons & PlayerButtons.Use) != 0 && (Server.TickedTime - CooldownManager[player.UserId.Value]) > cooldown)
                    {
                        Dash(player);
                        CooldownManager[player.UserId.Value] = Server.TickedTime;
                    }
                }
            }
        }

        private void Dash(CCSPlayerController player)
        {
            if (player != null)
            {
                var pawn = player!.PlayerPawn.Get();
                //var buttons = player.Buttons;
                var directionVec = new Vector();
                NativeAPI.AngleVectors(pawn!.EyeAngles.Handle, directionVec.Handle, IntPtr.Zero,
                    IntPtr.Zero);

                // Always shoot us up a little bit if were on the ground and not aiming up.
                if (directionVec.Z < 0.275)
                {
                    directionVec.Z = 0.275f;
                }

                directionVec *= 1000;

                pawn!.AbsVelocity.X = directionVec.X;
                pawn.AbsVelocity.Y = directionVec.Y;
                pawn.AbsVelocity.Z = directionVec.Z;
            }
        }
    }
}
